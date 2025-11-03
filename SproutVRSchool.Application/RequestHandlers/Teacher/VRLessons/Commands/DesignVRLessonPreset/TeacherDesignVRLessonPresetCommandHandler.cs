using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SproutVRSchool.Application.Abstractions.FileServices;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Exceptions;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Lessons;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Application.RequestHandlers.Teacher.VRLessons.Commands.DesignVRLessonPreset;

public sealed class TeacherDesignVRLessonPresetCommandHandler(
    IUnitOfWork uow,
    ILocalStorageService localStorageService,
    ILogger<TeacherDesignVRLessonPresetCommandHandler> logger
    ) : IRequestHandler<TeacherDesignVRLessonPresetCommand, Unit>
{
    public async Task<Unit> Handle(TeacherDesignVRLessonPresetCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch the VR Lesson and its realted Tasks (will all include)
        var spec = new VRLessonSpecification(request.VRLessonId);
        VRLesson vrLesson = await uow.Repository<VRLesson>().GetEntityBySpec(spec);

        // 2. If not found, throw not found exception
        if (vrLesson == null)
        {
            throw new SvrNotFoundException($"VR Lesson with ID {request.VRLessonId} was not found.");
        }

        // 3. Construct the preset object
        var presetFileObject = new PresetFileDto
        {
            Duration = vrLesson.MaxDuration.TotalSeconds,
            IsSequential = request.IsSequential,
            MapCode = vrLesson.Map.MapCode,
            VrTasks = vrLesson.VRTasks.Select(task =>
            {
                DesignVRLessonPresetTaskConfigRequestDto? configDto = request.TaskConfigs.FirstOrDefault(tc => tc.VRTaskId == task.Id);
                return new PresetVrTaskDto
                {
                    LocationCode = task.TaskLocation.LocationCode,
                    VrTaskId = task.Id.ToString(),
                    TaskNumber = task.TaskNumber,
                    TaskDescription = task.Description,
                    MapObject = new PresetMapObjectDto
                    {
                        ObjectCode = task.MapObject.ObjectCode,
                        ActivityType = new PresetActivityTypeDto
                        {
                            ActivityCode = task.ActivityType.ActivityCode,
                            Config = BuildTaskConfig(task.ActivityType.ActivityCode, configDto)
                        }
                    }
                };
            }).ToList()
        };

        // 4. Using Newtonsoft.json to serialize the preset object to JSON
        string jsonContent = JsonConvert.SerializeObject(presetFileObject,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
        );

        logger.LogInformation("Serlize params into json content: {JsonContent}", jsonContent);

        // 5. Convert to a stream for saving, store into the local, and return preset file
        byte[] byteArray = Encoding.UTF8.GetBytes(jsonContent);
        await using var stream = new MemoryStream(byteArray);

        string fileName = AppCts.FilePaths.FILE_NAME_PRESET_VR_LESSON;
        string relativePath = await localStorageService.SaveVrLessonPresetAsync(
            vrLesson.Lesson.TeacherId,
            vrLesson.LessonId,
            vrLesson.Id,
            fileName,
            stream
        );

        vrLesson.SetPresetFile(relativePath);
        logger.LogInformation("Relative Path: {RelativePath}", relativePath);

        uow.Repository<VRLesson>().Update(vrLesson);
        await uow.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Serialize params into json content: {JsonContent}", jsonContent);

        return Unit.Value;
    }

    /// <summary>
    /// A function to build a config schema based on activity type
    /// </summary>
    /// <param name="activityCode"></param>
    /// <param name="configDto"></param>
    /// <returns></returns>
    private static object BuildTaskConfig(string activityCode, DesignVRLessonPresetTaskConfigRequestDto? configDto)
    {
        if (configDto == null)
        {
            return null;
        }

        // quiz has question + answers
        // info has information
        // interact, grab has no config
        return activityCode.ToLower(System.Globalization.CultureInfo.CurrentCulture) switch
        {
            "quiz" => new { configDto.Question, configDto.Answers },
            "info" => new { configDto.Information },
            "interact" => null,
            "grab" => null,
            _ => null,
        };
    }
}

