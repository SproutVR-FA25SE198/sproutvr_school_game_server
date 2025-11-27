using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Domain.Entities;

namespace SproutVRSchool.Infrastructure.Repositories;

public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    // ===========================
    // === Fields & Props
    // ===========================

    private readonly TDbContext _dbContext;
    private readonly ConcurrentDictionary<string, object> _repositories = new();

    // ===========================
    // === Constructors
    // ===========================

    public UnitOfWork(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // ===========================
    // === Methods
    // ===========================

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public IGenericRepository<T> Repository<T>() where T : BaseEntity
    {
        string type = typeof(T).Name;

        return (IGenericRepository<T>)_repositories.GetOrAdd(type, t =>
        {
            // Get the unbound generic type definition: GenericRepository<>
            Type genericRepoDefinition = typeof(GenericRepository<>);

            // Call MakeGenericType on the definition, passing the desired entity type (T)
            Type repoType = genericRepoDefinition.MakeGenericType(typeof(T));

            // This will create an instance of that GenericRepository with DbContext injected
            return Activator.CreateInstance(repoType, _dbContext)
                ?? throw new InvalidOperationException(
                    string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "Could not create repository instance for {0}", t));
        });
    }
}
