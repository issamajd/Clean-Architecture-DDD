namespace DDD.SeedWork;

public interface IUnitOfWork
{
    T Repository<T, TEntity>() 
        where T : IRepository<TEntity> 
        where TEntity: Entity;

    Task BeginAsync();
    Task CompleteAsync();
    void Cancel();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}