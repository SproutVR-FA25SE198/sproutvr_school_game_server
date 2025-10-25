using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using SproutVRSchool.Domain.Entities.Subjects;

namespace SproutVRSchool.Application.RequestHandlers.Lessons.CreateLesson;

public record CreateLessonCommand : IRequest<Guid>
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
