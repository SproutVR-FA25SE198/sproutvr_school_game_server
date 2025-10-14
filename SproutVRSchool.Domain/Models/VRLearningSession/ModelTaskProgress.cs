namespace SproutVRSchool.Domain.Models.VRLearningSession;

public class ModelTaskProgress
{
    public string QuestionText { get; set; }
    public ModelTaskProgressStatus Status { get; set; }
    public bool? IsCorrect { get; set; }
    public string? AnswerText { get; set; }
    public DateTimeOffset CompletionTimeAtUtc { get; set; }
}
