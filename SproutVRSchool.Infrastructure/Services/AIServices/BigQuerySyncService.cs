using System.Globalization;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.BigQuery.V2;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.AIServices;
using SproutVRSchool.Domain.Entities;
using SproutVRSchool.Domain.Entities.Identities;
using SproutVRSchool.Domain.Entities.Lessons;
using SproutVRSchool.Domain.Entities.SystemSettings;
using SproutVRSchool.Domain.Entities.VRDeviceTaskProgresses;
using SproutVRSchool.Domain.Entities.VRLearningSessions;
using SproutVRSchool.Domain.Entities.VRLessons;
using SproutVRSchool.Domain.Entities.VRTasks;
using SproutVRSchool.Domain.Models.BigQueryModels;
using SproutVRSchool.Infrastructure.Data;

namespace SproutVRSchool.Infrastructure.Services.AIServices;

public class BigQuerySyncService : IBigQuerySyncService
{
    // =================================
    // === Fields
    // =================================

    private readonly BigQueryClient _client;
    private readonly string _datasetId;
    private string? _cachedOrgId;
    private readonly string _projectId;
    private readonly UserManager<UserAccount> _userManager;
    private readonly ILogger<BigQuerySyncService> _logger;
    private readonly SchoolServerDbContext _dbContext;

    // =================================
    // === Constructors
    // =================================

    public BigQuerySyncService(IConfiguration config, UserManager<UserAccount> userManager, ILogger<BigQuerySyncService> logger, SchoolServerDbContext dbContext)
    {
        _userManager = userManager;
        _logger = logger;
        _dbContext = dbContext;
        _projectId = config["BigQuery:ProjectId"] ?? string.Empty;
        _datasetId = config["BigQuery:DatasetId"] ?? string.Empty;
        _client = CreateBigQueryClient(config["BigQuery:CredentialsPath"] ?? string.Empty);
    }

    // =================================
    // === Methods
    // =================================

    private BigQueryClient CreateBigQueryClient(string credentialspath)
    {
        try
        {
            if (!string.IsNullOrEmpty(credentialspath))
            {
                var credential = GoogleCredential.FromFile(credentialspath);
                return BigQueryClient.Create(_projectId, credential);
            }
            else
            {
                return BigQueryClient.Create(_projectId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problem getting the key. Please add Google Credential.");
        }
        return null;
    }

    private async Task<string> GetOrganizationIdAsync()
    {
        if (!string.IsNullOrEmpty(_cachedOrgId))
        {
            return _cachedOrgId;
        }

        SchoolAdmin? admin = await _userManager.Users.OfType<SchoolAdmin>().FirstOrDefaultAsync();

        if (admin == null)
        {
            throw new Exception("LỖI CẤU HÌNH NGHIÊM TRỌNG: Database này chưa có record SchoolAdmin nào. Không thể xác định OrganizationId.");
        }

        if (admin.OrganizationId == Guid.Empty)
        {
            throw new Exception("LỖI DỮ LIỆU: OrganizationId trong bảng SchoolAdmin đang bị rỗng (Guid.Empty). Vui lòng kiểm tra lại Database.");
        }

        _cachedOrgId = admin.OrganizationId.ToString();
        return _cachedOrgId;
    }

    public async Task SyncAllTablesAsync()
    {
        try
        {
            if (_client == null)
            {
                return;
            }

            string currentOrgId = await GetOrganizationIdAsync();

            _logger.LogInformation("--- BẮT ĐẦU SYNC CHO TRƯỜNG: {CurrentOrgId} ---", currentOrgId);

            await SyncTableAsync<Lesson, LessonBqModel>("Lessons", "Lessons", currentOrgId,
                e => new LessonBqModel { Id = e.Id.ToString(), Name = e.Name, Status = e.Status.ToString(), CreatedAtUtc = e.CreatedAtUtc, Description = e.Description, ResourceRelativeFilePath = e.ResourceRelativeFilePath, SubjectId = e.SubjectId.ToString(), SubjectName = e.Subject.Name, TeacherId = e.TeacherId.ToString(), UpdatedAtUtc = e.UpdatedAtUtc, OrganizationId = currentOrgId });

            await SyncTableAsync<VRLesson, VRLessonBqModel>("VRLessons", "VRLessons", currentOrgId,
                e => new VRLessonBqModel { Id = e.Id.ToString(), Name = e.Name, LessonId = e.LessonId.ToString(), CreatedAtUtc = e.CreatedAtUtc, Description = e.Description, MapId = e.MapId.ToString(), MaxDuration = e.MaxDuration.ToString(), UpdatedAtUtc = e.UpdatedAtUtc, PresetJsonRelativeFilePath = e.PresetJsonRelativeFilePath, Status = e.Status.ToString(), OrganizationId = currentOrgId });

            await SyncTableAsync<VRTask, VRTaskBqModel>("VRTasks", "VRTasks", currentOrgId,
                e => new VRTaskBqModel { Id = e.Id.ToString(), Question = e.Question, VRLessonId = e.VRLessonId.ToString(), CreatedAtUtc = e.CreatedAtUtc, ActivityTypeId = e.ActivityTypeId.ToString(), ActivityTypeName = e.ActivityType.Name, MapObjectId = e.MapObjectId.ToString(), MapObjectName = e.MapObject.Name, TaskDescription = e.TaskDescription, TaskLocationId = e.TaskLocationId.ToString(), TaskNumber = e.TaskNumber, UpdatedAtUtc = e.UpdatedAtUtc, OrganizationId = currentOrgId });

            await SyncTableAsync<VRLearningSession, VRLearningSessionBqModel>("VRLearningSessions", "VRLearningSessions", currentOrgId,
                e => new VRLearningSessionBqModel { Id = e.Id.ToString(), ClassName = e.ClassName, StartTimeAtUtc = e.StartTimeAtUtc, DurationInMinutes = e.DurationInMinutes, CreatedAtUtc = e.CreatedAtUtc, OrganizationId = currentOrgId, VRLessonId = e.VRLessonId.ToString(), UpdatedAtUtc = e.UpdatedAtUtc, EndTimeAtUtc = e.EndTimeAtUtc, Status = (int)e.Status, TeacherId = e.TeacherId.ToString() });

            await SyncTableAsync<VRDeviceTaskProgress, VRDeviceTaskProgressBqModel>("VRDeviceTaskProgresses", "VRDeviceTaskProgresses", currentOrgId,
                e => new VRDeviceTaskProgressBqModel { Id = e.Id.ToString(), StudentName = e.StudentName, IsCompleted = e.IsCompleted, IsCorrect = e.IsCorrect, CreatedAtUtc = e.CreatedAtUtc, CompletionTimeAtUtc = e.CompletionTimeAtUtc, UpdatedAtUtc = e.UpdatedAtUtc, OrganizationId = currentOrgId, VRDeviceId = e.VRDeviceId.ToString(), VRLearningSessionId = e.VRLearningSessionId.ToString(), VRTaskId = e.VRTaskId.ToString() });

            _logger.LogInformation("--- KẾT THÚC SYNC CHO TRƯỜNG: {CurrentOrgId} ---", currentOrgId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sync thất bại. Server này sẽ thử lại vào lần sau.");
        }
    }

    private async Task SyncTableAsync<TEntity, TBqModel>(
        string dbTableName,
        string bqTableName,
        string orgId,
        Func<TEntity, TBqModel> mapFunc)
        where TEntity : BaseEntity
        where TBqModel : BaseBqModel
    {
        try
        {
            DateTimeOffset lastSyncTime = await GetLastSyncTimeAsync(dbTableName);
            List<TEntity> newRecords;

            if (typeof(TEntity) == typeof(VRTask))
            {
                newRecords = await _dbContext.Set<VRTask>()
                    .AsNoTracking()
                    .Include(vt => vt.ActivityType)
                    .Include(vt => vt.MapObject)
                    .Where(x => x.CreatedAtUtc > lastSyncTime)
                    .OrderBy(x => x.CreatedAtUtc)
                    .Take(2000)
                    .Cast<TEntity>()
                    .ToListAsync();
            }
            else if (typeof(TEntity) == typeof(Lesson))
            {
                newRecords = await _dbContext.Set<Lesson>()
                    .AsNoTracking()
                    .Include(l => l.Subject)
                    .Where(x => x.CreatedAtUtc > lastSyncTime)
                    .OrderBy(x => x.CreatedAtUtc)
                    .Take(2000)
                    .Cast<TEntity>()
                    .ToListAsync();
            }
            else
            {
                newRecords = await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(x => x.CreatedAtUtc > lastSyncTime)
                .OrderBy(x => x.CreatedAtUtc)
                .Take(2000)
                .ToListAsync();
            }

            if (newRecords.Any())
            {
                var rows = new List<BigQueryInsertRow>();
                foreach (TEntity record in newRecords)
                {
                    TBqModel model = mapFunc(record);
                    model.OrganizationId = orgId;
                    BigQueryInsertRow row = model.ToBigQueryRow();
                    rows.Add(row);
                }

                await _client.InsertRowsAsync(_datasetId, bqTableName, rows);

                DateTimeOffset newCheckpoint = newRecords.Max(x => x.CreatedAtUtc);
                await SaveLastSyncTimeAsync(dbTableName, newCheckpoint);

                _logger.LogInformation("[{DbTableName}] Đã sync {Count} dòng mới lên BigQuery.", dbTableName, rows.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi sync bảng {DbTableName}", dbTableName);
        }
    }

    private async Task<DateTimeOffset> GetLastSyncTimeAsync(string dbTableName)
    {
        string settingKey = $"LastSync_{dbTableName}";

        SystemSetting? setting = await _dbContext.SystemSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Key == settingKey);

        if (setting == null || string.IsNullOrEmpty(setting.Value))
        {
            return new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
        }

        if (DateTimeOffset.TryParse(setting.Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset result))
        {
            return result.ToUniversalTime();
        }

        return DateTimeOffset.MinValue;
    }

    private async Task SaveLastSyncTimeAsync(string dbTableName, DateTimeOffset newCheckpoint)
    {
        string settingKey = $"LastSync_{dbTableName}";

        SystemSetting? setting = await _dbContext.SystemSettings
            .FirstOrDefaultAsync(x => x.Key == settingKey);

        if (setting == null)
        {
            setting = new SystemSetting
            {
                Key = settingKey,
                Value = newCheckpoint.ToString("o"),
                Description = $"Thời gian đồng bộ cuối cùng của bảng {dbTableName} lên BigQuery"
            };
            _dbContext.SystemSettings.Add(setting);
        }
        else
        {
            setting.Value = newCheckpoint.ToString("o");
        }

        await _dbContext.SaveChangesAsync();
    }
}

