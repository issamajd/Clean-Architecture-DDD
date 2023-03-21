using System.Linq.Expressions;

namespace DDD.Core.Domain;

public interface IRepository<TEntity> where TEntity : Entity
{
    IQueryable<TEntity> WithDetails();

    IQueryable<TEntity> WithDetails(
        params Expression<Func<TEntity, object>>[] propertySelectors);

    /// <summary>
    /// Get a single entity by the given <paramref name="predicate" />.
    /// <para>
    /// It returns null if there is no entity with the given <paramref name="predicate" />.
    /// It throws <see cref="T:System.InvalidOperationException" /> if there are multiple entities with the given <paramref name="predicate" />.
    /// </para>
    /// </summary>
    /// <param name="predicate">A condition to find the entity</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a single entity by the given <paramref name="predicate" />.
    /// <para>
    /// It throws <see cref="T:DDD.Identity.SeedWork.EntityNotFoundException" /> if there is no entity with the given <paramref name="predicate" />.
    /// It throws <see cref="T:System.InvalidOperationException" /> if there are multiple entities with the given <paramref name="predicate" />.
    /// </para>
    /// </summary>
    /// <param name="predicate">A condition to filter entities</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    /// <summary>Gets a list of all the entities.</summary>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Entity</returns>
    Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default);

    /// <summary>Gets total count of all entities.</summary>
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetPagedListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of entities by the given <paramref name="predicate" />.
    /// </summary>
    /// <param name="predicate">A condition to filter the entities</param>
    /// <param name="includeDetails">Set true to include details (sub-collections) of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes many entities by the given <paramref name="predicate" />.
    /// <para>
    /// Please note: This may cause major performance problems if there are too many entities returned for a
    /// given predicate and the database provider doesn't have a way to efficiently delete many entities.
    /// </para>
    /// </summary>
    /// <param name="predicate">A condition to filter entities</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task DeleteAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <param name="entity">Entity to be deleted</param>
    void Delete(TEntity entity);

    /// <summary>
    /// Add a new entity
    /// </summary>
    /// <param name="entity">Added entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">Entity</param>
    TEntity Update(TEntity entity);
}