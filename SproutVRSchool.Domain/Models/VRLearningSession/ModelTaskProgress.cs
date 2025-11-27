namespace SproutVRSchool.Domain.Models.VRLearningSession;

public class ModelTaskProgress
{
    public string VRTaskId { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsCorrect { get; set; }
    public DateTimeOffset? CompletionTimeAtUtc { get; set; }
    public ModelTaskProgressStatus Status { get; set; } = ModelTaskProgressStatus.Uncompleted;
}
