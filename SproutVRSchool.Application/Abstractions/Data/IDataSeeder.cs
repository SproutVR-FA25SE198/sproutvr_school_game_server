using SproutVRSchool.Domain.Entities;

namespace SproutVRSchool.Application.Abstractions.Data;

public interface IDataSeeder
{
    void AddRelativePath<T>(string relativefilePath) where T : BaseEntity;
    Task SeedAllTablesAsync();
}
