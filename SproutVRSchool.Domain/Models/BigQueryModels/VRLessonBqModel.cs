using Google.Cloud.BigQuery.V2;

namespace SproutVRSchool.Domain.Models.BigQueryModels;

public class VRLessonBqModel : BaseBqModel
{
    public string Id { get; set; }
    public string LessonId { get; set; }
    public string MapId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string MaxDuration { get; set; }
    public string? PresetJsonRelativeFilePath { get; set; }
    public string Status { get; set; }

    public override BigQueryInsertRow ToBigQueryRow()
    {
        var row = new BigQueryInsertRow(Id);
        row["Id"] = Id;
        row["LessonId"] = LessonId;
        row["MapId"] = MapId;
        row["Name"] = Name;
        row["Description"] = Description;
        row["MaxDuration"] = MaxDuration;
        row["PresetJsonRelativeFilePath"] = PresetJsonRelativeFilePath ?? string.Empty;
        row["Status"] = Status;
        row["CreatedAtUtc"] = CreatedAtUtc;
        row["UpdatedAtUtc"] = UpdatedAtUtc;
        row["OrganizationId"] = OrganizationId;
        return row;
    }
}
