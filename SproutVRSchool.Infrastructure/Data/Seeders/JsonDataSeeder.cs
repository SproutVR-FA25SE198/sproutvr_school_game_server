using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SproutVRSchool.Application.Abstractions.Data;
using SproutVRSchool.Application.Abstractions.FileServices;
using SproutVRSchool.Application.Exceptions;
using SproutVRSchool.Domain.Entities;

namespace SproutVRSchool.Infrastructure.Data.Seeders;

public class JsonDataSeeder<TDbContext> : IDataSeeder
    where TDbContext : DbContext
{
    // =====================================
    // === Fields & Props
    // =====================================

    private readonly IFileReader _fileReader;
    private readonly List<(string relativeFilePath, Type entityType)> _seedFileInfors;
    private readonly TDbContext _dbContext;

    // =====================================
    // === Constructors
    // =====================================

    public JsonDataSeeder(IFileReader fileReader, TDbContext dbContext)
    {
        _fileReader = fileReader;
        _dbContext = dbContext;
        _seedFileInfors = new();
    }

    // =====================================
    // === Methods
    // =====================================

    /// <summary>
    /// Add the relative path of the json file as longh as the entity type
    /// </summary>
    /// <param name="relativeFilePath"></param>
    public void AddRelativePath<T>(string relativeFilePath) where T : BaseEntity
    {
        _seedFileInfors.Add((relativeFilePath, typeof(T)));
    }

    /// <summary>
    /// Parsing the json file into the list object with the specific entity type
    /// </summary>
    /// <param name="absoluteFilePath"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private async Task<IEnumerable<object>> ParseJsonToObject(string absoluteFilePath, Type entityType)
    {
        try
        {
            string json = await _fileReader.ReadAbsoluteFilePathAsync(absoluteFilePath);
            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Include,
                MissingMemberHandling = MissingMemberHandling.Error,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            };

            Type listType = typeof(List<>).MakeGenericType(entityType);
            var data = JsonConvert.DeserializeObject(json, listType, settings) as IEnumerable<object>;

            return data ?? [];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// Seed all entities from json files
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task SeedAllTablesAsync()
    {
        // If no path provided, return
        if (!_seedFileInfors.Any())
        {
            throw new FileNotFoundException("Does not have files");
        }

        // Seed data based on entity
        if (await _dbContext.Database.CanConnectAsync())
        {
            foreach ((string relativeFilePath, Type entityType) in _seedFileInfors)
            {
                // base directory
                // /app in Docker
                // /bin/Debug/net9.0 in local
                string absoluteFilePath = Path.Combine(AppContext.BaseDirectory, relativeFilePath);

                // Using reflection to call the genericMethod method
                System.Reflection.MethodInfo? method = typeof(DbContext).GetMethod("Set", Type.EmptyTypes);
                System.Reflection.MethodInfo? genericMethod = method?.MakeGenericMethod(entityType);
                object? dbSet = genericMethod?.Invoke(_dbContext, null);
                IEnumerable<object> entities = await ParseJsonToObject(absoluteFilePath, entityType);

                // Skip seeding if there are existing records
                dynamic queryable = dbSet as IQueryable;
                if (await EntityFrameworkQueryableExtensions.AnyAsync(queryable))
                {
                    continue;
                }

                // reflect to get the AddRange method
                System.Reflection.MethodInfo? addRangeMethod = dbSet?.GetType().GetMethod("AddRange", new[] { typeof(IEnumerable<>).MakeGenericType(entityType) });
                if (addRangeMethod is null)
                {
                    throw new InvalidOperationException($"Cannot find AddRange method for type {entityType.Name}");
                }
                addRangeMethod.Invoke(dbSet, new object[] { entities });
            }
        }

        // Save change to the database
        await _dbContext.SaveChangesAsync();
    }

    public async Task SeedSingleFileAsync<T>(string absoluteFilePath, DbSet<T> dbSet) where T : class
    {
        if (!File.Exists(absoluteFilePath))
        {
            throw new SvrResourceNotFoundException($"Seed file not found: {absoluteFilePath}");
        }

        string jsonContent = await File.ReadAllTextAsync(absoluteFilePath);
        var settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Include,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        };

        List<T>? entities = JsonConvert.DeserializeObject<List<T>>(jsonContent, settings);
        if (entities is null || !entities.Any())
        {
            throw new SvrResourceNotFoundException($"No data found in seed file: {absoluteFilePath}");
        }

        // Save change to the database
        dbSet.AddRange(entities);
        await _dbContext.SaveChangesAsync();
    }
}
