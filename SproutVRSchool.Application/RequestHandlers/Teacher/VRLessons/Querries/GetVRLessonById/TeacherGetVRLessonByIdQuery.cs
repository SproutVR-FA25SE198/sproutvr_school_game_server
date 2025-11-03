using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Teacher.VRLessons.Querries.GetVRLessonById;

public record TeacherGetVRLessonByIdQuery(Guid id)
    : IRequest<TeacherGetVRLessonByIdResponseDto>
{
}
