using System.Text.Json.Serialization;
using MediatR;
using Newtonsoft.Json;

namespace SproutVRSchool.Application.RequestHandlers.VRLessons.DesignVRLessonPreset;

// ================================
// === For Command
// ================================
public record DesignVRLessonPresetCommand
(
    bool IsSequential,
    List<DesignVRLessonPresetTaskConfigRequestDto> TaskConfigs
) : IRequest<Unit>
{
    public Guid VRLessonId { get; set; }
}
public record DesignVRLessonPresetTaskConfigRequestDto(
    Guid VRTaskId,
    string? Question,
    List<DesignVRLessonPresetAnswerRequestDto>? Answers,
    string? Information);

public record DesignVRLessonPresetAnswerRequestDto(string Text, bool IsCorrect);

// ================================
// === For Preset File
// ================================

/// <summary>
/// This is the root DTO that represents the entire preset.json file.
/// </summary>
public class PresetFileDto
{
    public string MapCode { get; set; }
    public double Duration { get; set; }
    public bool IsSequential { get; set; }
    public List<PresetVrTaskDto> VrTasks { get; set; } = [];
}

/// <summary>
/// Represents a single task ("vrTasks" array item) in the preset.
/// </summary>
public class PresetVrTaskDto
{
    public string LocationCode { get; set; }
    public string VrTaskId { get; set; }
    public int TaskNumber { get; set; }
    public string TaskDescription { get; set; }
    public PresetMapObjectDto MapObject { get; set; }
}

/// <summary>
/// Represents the map object associated with a task.
/// </summary>
public class PresetMapObjectDto
{
    public string ObjectCode { get; set; }
    public PresetActivityTypeDto ActivityType { get; set; }
}

/// <summary>
/// Represents the activity type and its dynamic _configuration.
/// </summary>
public class PresetActivityTypeDto
{
    public string ActivityCode { get; set; }

    // It will be null for simple activities like 'grab', 'interact'
    // If set null, then this property will be ignored during JSON serialization
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public object? Config { get; set; }
}
