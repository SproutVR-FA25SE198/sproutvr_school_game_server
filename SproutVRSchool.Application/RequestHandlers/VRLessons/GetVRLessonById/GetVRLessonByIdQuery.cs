using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.VRLessons.GetVRLessonById;

public record GetVRLessonByIdQuery(Guid id)
    : IRequest<GetVRLessonByIdResponseDto>
{
}
