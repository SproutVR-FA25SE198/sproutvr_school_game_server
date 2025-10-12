using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Domain.Entities;

namespace SproutVRSchool.Application.Abstractions.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    IGenericRepository<T> Repository<T>() where T : BaseEntity;
}
