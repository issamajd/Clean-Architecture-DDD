using DDD.SeedWork;
using Microsoft.Extensions.DependencyInjection;

namespace DDD;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;

    public UnitOfWork(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _appDbContext.SaveChangesAsync(cancellationToken);
    }
}