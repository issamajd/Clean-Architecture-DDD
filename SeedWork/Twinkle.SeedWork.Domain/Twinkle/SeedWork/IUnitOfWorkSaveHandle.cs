namespace Twinkle.SeedWork;

public interface IUnitOfWorkSaveHandle
{
    Task CompleteAsync();
    void Cancel();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}