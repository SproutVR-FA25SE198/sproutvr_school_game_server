using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Domain.Entities;

namespace SproutVRSchool.Application.Abstractions.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
