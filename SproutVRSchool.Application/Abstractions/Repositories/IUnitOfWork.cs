using SproutVRSchool.Domain.Entities;

namespace SproutVRSchool.Application.Abstractions.Repositories;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T> Repository<T>() where T : BaseEntity;
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}
