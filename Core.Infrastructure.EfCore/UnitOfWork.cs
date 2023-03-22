using DDD.Core.Domain;
using Microsoft.EntityFrameworkCore.Storage;

namespace DDD.Core.Infrastructure.EfCore;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbContext _appDbContext;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(IDbContext dbContext)
    {
        _appDbContext = dbContext;
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