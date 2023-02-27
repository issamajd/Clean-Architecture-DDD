using DDD.Identity.SeedWork;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Identity;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;
    private readonly IServiceProvider _serviceProvider;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(AppDbContext appDbContext, IServiceProvider serviceProvider)
    {
        _appDbContext = appDbContext;
        _serviceProvider = serviceProvider;
    }

    public T Repository<T, TEntity>()
        where T : IRepository<TEntity>
        where TEntity : Entity
    {
        return _serviceProvider.GetRequiredService<T>();
    }

    public Task BeginAsync() => BeginTransactionAsync();

    public Task CompleteAsync() => CommitTransactionAsync();

    public void Cancel() => RollbackTransaction();
    
    private async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null)
            return;

        _currentTransaction = await _appDbContext.Database.BeginTransactionAsync();
    }

    private async Task CommitTransactionAsync()
    {
        if (_currentTransaction == null) throw new ArgumentNullException(nameof(_currentTransaction));
        try
        {
            await SaveChangesAsync();
            await _currentTransaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }

    private void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally //To make sure that the current transaction will be disposed of
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _appDbContext.SaveChangesAsync(cancellationToken);
    }
}