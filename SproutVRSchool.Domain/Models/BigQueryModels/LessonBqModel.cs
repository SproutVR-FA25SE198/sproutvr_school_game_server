using Google.Cloud.BigQuery.V2;

namespace SproutVRSchool.Domain.Models.BigQueryModels;
public class LessonBqModel : BaseBqModel
{
    public string Id { get; set; }
    public string SubjectId { get; set; }
    public string SubjectName { get; set; }
    public string TeacherId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ResourceRelativeFilePath { get; set; }
    public string Status { get; set; }

    public override BigQueryInsertRow ToBigQueryRow()
    {
        var row = new BigQueryInsertRow(Id);
        row["Id"] = Id;
        row["SubjectId"] = SubjectId;
        row["SubjectName"] = SubjectName;
        row["TeacherId"] = TeacherId;
        row["Name"] = Name;
        row["Description"] = Description;
        row["ResourceRelativeFilePath"] = ResourceRelativeFilePath;
        row["Status"] = Status;
        row["CreatedAtUtc"] = CreatedAtUtc;
        row["UpdatedAtUtc"] = UpdatedAtUtc;
        row["OrganizationId"] = OrganizationId;
        return row;
    }
}
