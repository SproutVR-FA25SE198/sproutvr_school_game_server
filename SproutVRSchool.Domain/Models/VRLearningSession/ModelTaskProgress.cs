namespace SproutVRSchool.Domain.Models.VRLearningSession;

public class ModelTaskProgress
{
    public ModelTaskProgressStatus Status { get; set; } = ModelTaskProgressStatus.Uncompleted;
    public bool IsCompleted { get; set; }
    public bool IsCorrect { get; set; }
    public DateTimeOffset? CompletionTimeAtUtc { get; set; }
}
