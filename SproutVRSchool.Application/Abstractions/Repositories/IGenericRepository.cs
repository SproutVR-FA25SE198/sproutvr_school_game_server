using Microsoft.EntityFrameworkCore;
using SproutVRSchool.Domain.Entities;

namespace SproutVRSchool.Application.Abstractions.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<(IReadOnlyList<T> Data, int Count)> ListAllAsync();
    Task<(IReadOnlyList<T> Data, int Count)> ListAsync(ISpecification<T> spec);
    Task<T> GetEntityByIdAsync(Guid id);
    Task<T> GetEntityBySpec(ISpecification<T> spec);
    Task<int> CountAsync(ISpecification<T> spec);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    bool Exists(Guid id);
    Task<bool> SaveAllAsync();
    void Attach(T t);
    EntityState GetEntityState(T entity);
}
