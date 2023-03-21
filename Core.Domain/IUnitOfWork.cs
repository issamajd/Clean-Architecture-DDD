namespace DDD.Core.Domain;

public interface IUnitOfWork
{
    Task BeginAsync();
    Task CompleteAsync();
    void Cancel();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}