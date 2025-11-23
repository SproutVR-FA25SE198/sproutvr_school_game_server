using Google.Cloud.BigQuery.V2;

namespace SproutVRSchool.Domain.Models.BigQueryModels;
public abstract class BaseBqModel
{
    public string OrganizationId { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }
    public abstract BigQueryInsertRow ToBigQueryRow();
}
