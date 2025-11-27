using Google.Cloud.BigQuery.V2;

namespace SproutVRSchool.Domain.Models.BigQueryModels;

public class VRLearningSessionBqModel : BaseBqModel
{
    public string Id { get; set; }
    public string VRLessonId { get; set; }
    public string TeacherId { get; set; }
    public string ClassName { get; set; }
    public DateTimeOffset StartTimeAtUtc { get; set; }
    public DateTimeOffset EndTimeAtUtc { get; set; }
    public int DurationInMinutes { get; set; }
    public int Status { get; set; }

    public override BigQueryInsertRow ToBigQueryRow()
    {
        var row = new BigQueryInsertRow(Id);
        row["Id"] = Id;
        row["VRLessonId"] = VRLessonId;
        row["TeacherId"] = TeacherId;
        row["ClassName"] = ClassName;
        row["StartTimeAtUtc"] = StartTimeAtUtc;
        row["EndTimeAtUtc"] = EndTimeAtUtc;
        row["DurationInMinutes"] = DurationInMinutes;
        row["Status"] = Status;
        row["CreatedAtUtc"] = CreatedAtUtc;
        row["UpdatedAtUtc"] = UpdatedAtUtc;
        row["OrganizationId"] = OrganizationId;
        return row;
    }
}
