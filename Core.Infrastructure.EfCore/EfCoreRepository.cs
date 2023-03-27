using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using DDD.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace DDD.Core.Infrastructure.EfCore;

public abstract class EfCoreRepository<TEntity, TDbContext> : IRepository<TEntity>
    where TEntity : Entity
    where TDbContext : IDbContext
{
    private readonly TDbContext _dbContext;

    protected EfCoreRepository(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual IQueryable<TEntity> WithDetails()
    {
        return _dbContext.Set<TEntity>().AsQueryable();
    }

    public virtual IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[]? propertySelectors)
    {
        return IncludeDetails(
            _dbContext.Set<TEntity>().AsQueryable(),
            propertySelectors
        );
    }

    private static IQueryable<TEntity> IncludeDetails(
        IQueryable<TEntity> query,
        Expression<Func<TEntity, object>>[]? propertySelectors)
    {
        if (propertySelectors is not { Length: > 0 }) return query;
        return propertySelectors.Aggregate(query,
            (current, propertySelector) =>
                current.Include(propertySelector));
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await WithDetails().Where(predicate).SingleOrDefaultAsync(cancellationToken: cancellationToken)
            : await _dbContext.Set<TEntity>().Where(predicate)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(predicate, includeDetails, cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity));
        }

        return entity;
    }

    public async Task<List<TEntity>> GetListAsync(bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await WithDetails()
                .ToListAsync(cancellationToken: cancellationToken)
            : await _dbContext.Set<TEntity>()
                .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().CountAsync(cancellationToken: cancellationToken);
    }
    public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().CountAsync(predicate:predicate, cancellationToken: cancellationToken);
    }

    public async Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var queryable = includeDetails ? WithDetails() : _dbContext.Set<TEntity>().AsQueryable();

        sorting = sorting.Trim();
        if (sorting is { Length: > 0 })
            queryable = DynamicQueryableExtensions.OrderBy(queryable, sorting);

        return await queryable.Skip(skipCount).Take(maxResultCount)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await WithDetails()
                .Where(predicate)
                .ToListAsync(cancellationToken: cancellationToken)
            : await _dbContext.Set<TEntity>()
                .Where(predicate)
                .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.Set<TEntity>().Where(predicate)
            .ToListAsync(cancellationToken: cancellationToken);
        _dbContext.RemoveRange(entities);
    }

    public void Delete(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
    }

    public async Task<TEntity> AddAsync(TEntity entity,
        CancellationToken cancellationToken = default)
    {
        return (await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken)).Entity;
    }

    public TEntity Update(TEntity entity)
    {
        _dbContext.Attach(entity);
        return _dbContext.Update(entity).Entity;
    }
}