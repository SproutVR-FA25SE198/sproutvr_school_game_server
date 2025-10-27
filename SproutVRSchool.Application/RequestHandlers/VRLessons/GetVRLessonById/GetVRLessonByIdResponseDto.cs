using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.VRLessons.GetVRLessonById;

public record GetVRLessonByIdResponseDto
{
    public Guid Id { get; init; }
    public GetVRLessonByIdLessonResponseDto Lesson { get; init; }
    public GetVRLessonByIdMapResponseDto Map { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public TimeSpan Duration { get; init; }
    public string? PresetJsonRelativeFilePath { get; init; }
    public StatusDto Status { get; init; }

    // Nested list of tasks
    public List<GetVRLessonByIdTaskResponseDto> Tasks { get; init; } = [];
}

public class GetVRLessonByIdLessonResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
}

public record GetVRLessonByIdTaskResponseDto
{
    public Guid Id { get; init; }
    public GetVRLessonByIdTaskLocationResponseDto TaskLocation { get; init; }
    public GetVRLessonByIdMapObjectResponseDto MapObject { get; init; }
    public GetVRLessonByIdActivityTypeResponseDto ActivityType { get; init; }
    public int TaskNumber { get; init; }
    public string Description { get; init; }
}

public class GetVRLessonByIdActivityTypeResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string ActivityCode { get; init; }
}

public class GetVRLessonByIdMapObjectResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string ObjectCode { get; init; }
}

public class GetVRLessonByIdTaskLocationResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string LocationCode { get; init; }
}

public record GetVRLessonByIdMapResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string MapCode { get; init; }
}
