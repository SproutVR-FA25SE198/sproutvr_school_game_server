using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace SproutVRSchool.Application.Abstractions.Repositories;

/// <summary>
/// Custom specifications for entities.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> ThenIncludes { get; }
    IQueryable<T> ApplyCriteria(IQueryable<T> query);
    Expression<Func<T, object>> OrderBy { get; }
    Expression<Func<T, object>> OrderByDescending { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}
