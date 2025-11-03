using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using SproutVRSchool.Domain.Entities.Subjects;

namespace SproutVRSchool.Application.RequestHandlers.Teacher.Lessons.Commands.CreateLesson;

public record TeacherCreateLessonCommand : IRequest<Guid>
{
    public Guid SubjectId { get; init; }
    public Guid TeacherId { get; init; }

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Name { get; init; }

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Description { get; init; }

    // Resource file can be null 
    public IFormFile? ResourceFile { get; init; }
}
