namespace SproutVRSchool.Application.Abstractions.AIServices;

public interface IBigQuerySyncService
{
    Task SyncAllTablesAsync();

}
