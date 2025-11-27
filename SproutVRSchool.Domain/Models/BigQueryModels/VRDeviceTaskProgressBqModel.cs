using Google.Cloud.BigQuery.V2;

namespace SproutVRSchool.Domain.Models.BigQueryModels;

public class VRDeviceTaskProgressBqModel : BaseBqModel
{
    public string Id { get; set; }
    public string VRDeviceId { get; set; }
    public string VRTaskId { get; set; }
    public string VRLearningSessionId { get; set; }
    public string StudentName { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsCorrect { get; set; }
    public DateTimeOffset? CompletionTimeAtUtc { get; set; }

    public override BigQueryInsertRow ToBigQueryRow()
    {
        var row = new BigQueryInsertRow(Id);
        row["Id"] = Id;
        row["VRDeviceId"] = VRDeviceId;
        row["VRTaskId"] = VRTaskId;
        row["VRLearningSessionId"] = VRLearningSessionId;
        row["StudentName"] = StudentName;
        row["IsCompleted"] = IsCompleted;
        row["IsCorrect"] = IsCorrect;
        row["CompletionTimeAtUtc"] = CompletionTimeAtUtc;
        row["CreatedAtUtc"] = CreatedAtUtc;
        row["UpdatedAtUtc"] = UpdatedAtUtc;
        row["OrganizationId"] = OrganizationId;
        return row;
    }
}
