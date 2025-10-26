using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SproutVRSchool.Application.RequestHandlers.Devices.GetVRDeviceDetails;

namespace SproutVRSchool.Application.RequestHandlers.Lessons.GetLessonById;

public record GetLessonByIdQuery(Guid Id)
    : IRequest<GetLessonByIdResponseDto>
{
}
