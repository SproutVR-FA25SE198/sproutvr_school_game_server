using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace SproutVRSchool.Application.RequestHandlers.Teacher.Lessons.Commands.UpdateLesson;

public record TeacherUpdateLessonCommand : IRequest<Unit>
{
    public Guid LessonId { get; set; }

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string? Name { get; init; }

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string? Description { get; init; }
    public IFormFile? ResourceFile { get; init; }
}
