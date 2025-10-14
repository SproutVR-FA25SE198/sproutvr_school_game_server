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

    private readonly DbContext _context;

    // ===========================
    // === Constructors
    // ===========================

    public GenericRepository(DbContext context)
    {
        _context = context;
    }

    // ===========================
    // === Methods
    // ===========================



    /// <summary>
    /// Gets individual item based on its ID asynchronously of type T.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The task result contains an item with the specified ID</returns>
    public async Task<T> GetEntityByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    /// <summary>
    /// Gets an item with a specification asynchronously of type T.
    /// </summary>
    /// <param name="spec"></param>
    /// <returns>The task result contains an item based on a specification</returns>
    public async Task<T> GetEntityBySpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Asynchronously retrieves all entities of type T from the data source.
    /// </summary>
    /// <returns>A tuple containing a read-only list of all entities of type T and the total number of entities retrieved. The
    /// list will be empty and the count will be 0 if no entities are found.</returns>
    public async Task<(IReadOnlyList<T> Data, int Count)> ListAllAsync()
    {
        List<T> list = await _context.Set<T>().ToListAsync();
        int count = list.Count;

        return (list, count);
    }

    /// <summary>
    /// Asynchronously retrieves a list of entities that satisfy the specified criteria, along with the total count of
    /// matching entities.
    /// </summary>
    /// <param name="spec">The specification that defines the criteria used to filter and shape the returned entities. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with a read-only list of
    /// entities matching the specification and the total count of matching entities.</returns>
    public async Task<(IReadOnlyList<T> Data, int Count)> ListAsync(ISpecification<T> spec)
    {
        List<T> list = await ApplySpecification(spec).ToListAsync();
        int count = list.Count;

        return (list, count);
    }

    /// <summary>
    /// Counts the number of items with a specification asynchronously of type T.
    /// </summary>
    /// <param name="spec"></param>
    /// <returns>The task result contains an integer as the count of the items</returns>
    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        IQueryable<T> query = _context.Set<T>().AsQueryable();

        query = spec.ApplyCriteria(query);

        return await query.CountAsync();
    }

    /// <summary>
    /// Applies specification to an entity.
    /// </summary>
    /// <param name="spec"></param>
    /// <returns>A queryable set of the entity</returns>
    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }

    /// <summary>
    /// Adds new item of type T to the database.
    /// </summary>
    /// <param name="entity"></param>
    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    /// <summary>
    /// Deletes an item of type T from the database.
    /// </summary>
    /// <param name="entity"></param>
    public void Delete(T entity)
    {
        _context?.Set<T>().Remove(entity);
    }

    /// <summary>
    /// Tracks the changes of an item in the database, only save when SaveChanges method was called.
    /// </summary>
    /// <param name="entity"></param>
    public void Update(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    /// <summary>
    /// Save changes of the entries that were affected.
    /// </summary>
    /// <returns>A boolean true if one or more rows affected</returns>
    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Return a true or false on whether the entity exists
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool Exists(Guid id)
    {
        return _context.Set<T>().Any(x => x.Id == id);
    }

    public void Attach(T t)
    {
        _context.Set<T>().Attach(t);
    }

    public EntityState GetEntityState(T entity)
    {
        return _context.Entry(entity).State;
    }


}
