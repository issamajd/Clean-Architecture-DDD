using Microsoft.EntityFrameworkCore.Storage;

namespace Twinkle.SeedWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbContext _appDbContext;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(IDbContext dbContext)
    {
        _appDbContext = dbContext;
    }

    public async Task<IUnitOfWorkSaveHandle> BeginAsync()
    {
        var saveHandle = new UnitOfWorkSaveHandle(this);
        await BeginTransactionAsync();
        return saveHandle;
    }

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

    private Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _appDbContext.SaveChangesAsync(cancellationToken);
    }

    protected class UnitOfWorkSaveHandle : IUnitOfWorkSaveHandle
    {
        private readonly UnitOfWork _unitOfWork;

        public UnitOfWorkSaveHandle(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task CompleteAsync() => _unitOfWork.CommitTransactionAsync();

        public void Cancel() => _unitOfWork.RollbackTransaction();

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}