using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.GetRoomState;

public sealed class GetRoomStateResponseDto
{
    // ===========================
    // === Fields
    // ===========================

    public string VRLearningSessionId { get; init; }

    public TeacherInfoDto Teacher { get; init; } = default!;
    public VRLessonInfoDto VRLesson { get; init; } = default!;

    public string ClassName { get; init; } = default!;
    public string RoomCode { get; init; } = default!;
    public int DurationInSeconds { get; init; }
    public string Status { get; init; } = default!;

    public IReadOnlyList<DeviceInfoDto> Devices { get; init; } = new List<DeviceInfoDto>();

    // ===========================
    // === Methods
    // ===========================

    public static GetRoomStateResponse MapToGrpcResponse(GetRoomStateResponseDto dto)
    {
        return new GetRoomStateResponse
        {
            VrLearningSessionId = dto.VRLearningSessionId,
            ClassName = dto.ClassName,
            DurationInSeconds = dto.DurationInSeconds,
            RoomCode = dto.RoomCode,
            Vrlesson = new VRLessonInfo
            {
                VrLessonId = dto.VRLesson.VRLessonId,
                Name = dto.VRLesson.Name,
                Description = dto.VRLesson.Description,
                PresetJsonRelativeFilePath = dto.VRLesson.PresetJsonRelativeFilePath
            },

            Teacher = new TeacherInfo
            {
                TeacherId = dto.Teacher.TeacherId,
                TeacherName = dto.Teacher.TeacherName
            },

            Status = dto.Status,
            Devices =
            {
                dto.Devices.Select(deviceDto => new DeviceInfo
                {
                    VrDeviceSerialNumber = deviceDto.VRDeviceSerialNumber,
                    StudentName = deviceDto.StudentName,
                    Status = deviceDto.Status,
                    Tasks =
                    {
                        deviceDto.Tasks.Select(taskDto => new TaskInfo
                        {
                            VrTaskId = taskDto.VRTaskId,
                            IsCompleted = taskDto.IsCompleted,
                            IsCorrect = taskDto.IsCorrect,
                            CompletionTimeAtVietnam = taskDto.CompletionTimeAtVietnam ?? string.Empty,
                            Status = taskDto.Status
                        })
                    }
                })
            }
        };
    }

}

public sealed class TeacherInfoDto
{
    public string TeacherId { get; init; } = default!;
    public string TeacherName { get; init; } = default!;
}

public sealed class VRLessonInfoDto
{
    public string VRLessonId { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string PresetJsonRelativeFilePath { get; init; } = default!;
}

public sealed class DeviceInfoDto
{
    public string VRDeviceSerialNumber { get; init; } = default!;
    public string StudentName { get; init; } = default!;
    public string Status { get; init; } = default!;
    public IReadOnlyList<TaskInfoDto> Tasks { get; init; } = new List<TaskInfoDto>();
}

public sealed class TaskInfoDto
{
    public string VRTaskId { get; init; } = default!;
    public bool IsCompleted { get; init; }
    public bool IsCorrect { get; init; }
    public string? CompletionTimeAtVietnam { get; init; }
    public string Status { get; init; } = default!;
}
