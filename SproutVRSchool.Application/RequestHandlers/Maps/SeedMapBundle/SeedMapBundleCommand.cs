using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Server.Kestrel.Transport.NamedPipes;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.Data;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Exceptions;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.ActivityTypes;
using SproutVRSchool.Domain.Entities.MapObjects;
using SproutVRSchool.Domain.Entities.Maps;
using SproutVRSchool.Domain.Entities.MasterSubjects;
using SproutVRSchool.Domain.Entities.ObjectActivityTypes;
using SproutVRSchool.Domain.Entities.ObjectLocations;
using SproutVRSchool.Domain.Entities.Subjects;
using SproutVRSchool.Domain.Entities.TaskLocations;

namespace SproutVRSchool.Application.RequestHandlers.Maps.SeedMapBundle;

public sealed record SeedMapBundleResponseDto(string message);

public sealed record SeedMapBundleCommand(Guid MapId, string DownloadUrl)
    : IRequest<SeedMapBundleResponseDto>
{
}

public sealed class SeedMapBundleCommandHandler : IRequestHandler<SeedMapBundleCommand, SeedMapBundleResponseDto>
{
    // ================================
    // === Fields
    // ================================

    private readonly string _tempSeedingPath;
    private string _tempExtractedPath;
    private string _tempZipFilePath;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IDataSeeder _fileSeeder;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SeedMapBundleCommandHandler> _logger;

    // ================================
    // === Constructors
    // ================================

    public SeedMapBundleCommandHandler(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        IUnitOfWork unitOfWork,
        IServiceProvider serviceProvider,
        IDataSeeder fileSeeder,
        ILogger<SeedMapBundleCommandHandler> logger
    )
    {
        _httpClientFactory = httpClientFactory;
        _unitOfWork = unitOfWork;
        _fileSeeder = fileSeeder;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _tempSeedingPath = configuration["FileLocalStorageSettings:TempSeedingPath"]
            ?? throw new InvalidOperationException("TempSeedingPath _configuration is missing.");
    }

    // ================================
    // === Methods
    // ================================

    public async Task<SeedMapBundleResponseDto> Handle(SeedMapBundleCommand request, CancellationToken cancellationToken)
    {
        // 1. Currently, create a temp directory for seeding
        Directory.CreateDirectory(_tempSeedingPath);

        // 2. Check if MapId is alreay installed
        await CheckIfMapExistsAsync(request.MapId);

        // 3. Download the map bundle from the given URL
        string uniqueFileName = $"{request.MapId}.zip";
        _tempZipFilePath = Path.Combine(_tempSeedingPath, uniqueFileName);
        _tempExtractedPath = Path.Combine(_tempSeedingPath, $"{request.MapId}_extracted");
        await DownloadAndUnzipMapBundleAsync(request.DownloadUrl, cancellationToken);

        _logger.LogDebug("Successfully downloaded and extracted to {Path}", _tempExtractedPath);

        // 4. Process the extracted files and seed the map into the system
        await SeedDataFromFolderAsync(cancellationToken);

        // 5. Cleanup temp seeding folder after successfilly seeding
        CleanUp();

        return new SeedMapBundleResponseDto($"Successfully seeded map bundle with ID '{request.MapId}'.");
    }

    private void CleanUp()
    {
        // delete zip file path
        if (!string.IsNullOrEmpty(_tempZipFilePath) && File.Exists(_tempZipFilePath))
        {
            File.Delete(_tempZipFilePath);
            _logger.LogInformation("Cleaned up temp file: {ZipPath}", _tempZipFilePath);
        }

        if (!string.IsNullOrEmpty(_tempExtractedPath) && Directory.Exists(_tempExtractedPath))
        {
            // 'true' means "delete recursively" (the folder and everything inside it)
            Directory.Delete(_tempExtractedPath, true);
            _logger.LogInformation("Cleaned up temp folder: {ExtractPath}", _tempExtractedPath);
        }
    }

    private async Task SeedDataFromFolderAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        ISchoolServerDbContext dbContext = scope.ServiceProvider.GetRequiredService<ISchoolServerDbContext>();

        await using IDbContextTransaction transaction = await dbContext.BeginTransactionAsync(cancellationToken);
        try
        {
            // Seed .json file data into the database
            await _fileSeeder.SeedSingleFileAsync<MasterSubject>(Path.Combine(_tempExtractedPath, "MasterSubject.json"), dbContext.MasterSubjects);
            await _fileSeeder.SeedSingleFileAsync<Subject>(Path.Combine(_tempExtractedPath, "Subject.json"), dbContext.Subjects);
            await _fileSeeder.SeedSingleFileAsync<ActivityType>(Path.Combine(_tempExtractedPath, "ActivityType.json"), dbContext.ActivityTypes);
            await _fileSeeder.SeedSingleFileAsync<Map>(Path.Combine(_tempExtractedPath, "Map.json"), dbContext.Maps);
            await _fileSeeder.SeedSingleFileAsync<MapObject>(Path.Combine(_tempExtractedPath, "MapObject.json"), dbContext.MapObjects);
            await _fileSeeder.SeedSingleFileAsync<TaskLocation>(Path.Combine(_tempExtractedPath, "TaskLocation.json"), dbContext.TaskLocations);
            await _fileSeeder.SeedSingleFileAsync<ObjectActivityType>(Path.Combine(_tempExtractedPath, "ObjectActivityType.json"), dbContext.ObjectActivityTypes);
            await _fileSeeder.SeedSingleFileAsync<ObjectLocation>(Path.Combine(_tempExtractedPath, "ObjectLocation.json"), dbContext.ObjectLocations);

            // Save changes and commit transaction to DB
            await dbContext.SaveChangesAsync(cancellationToken);
            await dbContext.CommitTransactionAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            CleanUp();
            await dbContext.RollbackTransactionAsync(transaction, cancellationToken);
            throw new SvrInstallFailedException($"Failed to seed map data from folder '{_tempExtractedPath}': {ex.Message}");
        }
    }

    private async Task DownloadAndUnzipMapBundleAsync(string downloadUrl, CancellationToken cancellationToken)
    {
        try
        {
            using HttpClient client = _httpClientFactory.CreateClient();

            // Download the zip file
            var fileStream = new FileStream(
                _tempZipFilePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.ReadWrite);

            await using (Stream httpStream = await client.GetStreamAsync(downloadUrl, cancellationToken))
            {
                await httpStream.CopyToAsync(fileStream, cancellationToken);
            }

            // Release the file stream locking on .zip file
            await fileStream.FlushAsync(cancellationToken);
            await fileStream.DisposeAsync();

            ZipFile.ExtractToDirectory(_tempZipFilePath, _tempExtractedPath);
        }
        catch (Exception ex)
        {
            CleanUp();
            throw new SvrDownloadFailedException($"Failed to download or unzip the map bundle from '{downloadUrl}': {ex.Message}");
        }
    }

    /// <summary>
    /// Check if a map with the given ID already exists in the system and not seeding it again
    /// </summary>
    /// <param name="mapId"></param>
    /// <returns></returns>
    /// <exception cref="SvrAlreadyInstalledException"></exception>
    private async Task CheckIfMapExistsAsync(Guid mapId)
    {
        Map map = await _unitOfWork.Repository<Map>().GetEntityByIdAsync(mapId);

        if (map is not null)
        {
            throw new SvrAlreadyInstalledException($"Map with ID '{mapId}' is already installed.");
        }
    }
}
