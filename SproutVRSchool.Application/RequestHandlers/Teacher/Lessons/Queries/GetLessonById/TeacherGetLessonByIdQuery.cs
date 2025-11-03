using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Teacher.Lessons.Queries.GetLessonById;

public record TeacherGetLessonByIdQuery(Guid Id)
    : IRequest<TeacherGetLessonByIdQueryResponseDto>
{
}
