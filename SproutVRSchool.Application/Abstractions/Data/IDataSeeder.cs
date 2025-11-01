using Microsoft.EntityFrameworkCore;
using SproutVRSchool.Domain.Entities;

namespace SproutVRSchool.Application.Abstractions.Data;

public interface IDataSeeder
{
    void AddRelativePath<T>(string relativeFilePath) where T : BaseEntity;
    Task SeedAllTablesAsync();
    Task SeedSingleFileAsync<T>(string absoluteFilePath, DbSet<T> dbSet) where T : class;
}
