using Google.Cloud.BigQuery.V2;

namespace SproutVRSchool.Domain.Models.BigQueryModels;
public class VRTaskBqModel : BaseBqModel
{
    public string Id { get; set; }
    public string TaskLocationId { get; set; }
    public string MapObjectId { get; set; }
    public string MapObjectName { get; set; }
    public string ActivityTypeId { get; set; }
    public string ActivityTypeName { get; set; }
    public string VRLessonId { get; set; }
    public int TaskNumber { get; set; }
    public string? Question { get; set; }
    public string TaskDescription { get; set; }


    public override BigQueryInsertRow ToBigQueryRow()
    {
        var row = new BigQueryInsertRow(Id);
        row["Id"] = Id;
        row["TaskLocationId"] = TaskLocationId;
        row["MapObjectId"] = MapObjectId;
        row["ActivityTypeId"] = ActivityTypeId;
        row["MapObjectName"] = MapObjectName;
        row["ActivityTypeName"] = ActivityTypeName;
        row["VRLessonId"] = VRLessonId;
        row["TaskNumber"] = TaskNumber;
        row["Question"] = Question;
        row["TaskDescription"] = TaskDescription;
        row["CreatedAtUtc"] = CreatedAtUtc;
        row["UpdatedAtUtc"] = UpdatedAtUtc;
        row["OrganizationId"] = OrganizationId;
        return row;
    }
}
