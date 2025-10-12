using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Domain.Entities;

namespace SproutVRSchool.Infrastructure.Repositories;

public sealed class GenericRepository<T> : IGenericRepository<T>
    where T : BaseEntity
{
    // ===========================
    // === Fields
    // ===========================

    private readonly DbContext _dbContext;

    // ===========================
    // === Constructors
    // ===========================

    public GenericRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // ===========================
    // === Methods
    // ===========================

    public void Add(T entity)
    {
        _dbContext.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
        _dbContext.Set<T>().Update(entity);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<T>()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
