using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Lessons.GetLessonById;

public record GetLessonByIdCommandResponseDto(
    );

public sealed record GetLessonByIdCommand(
    Guid Id) : IRequest<GetLessonByIdCommandResponseDto>
{
}
