using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Twinkle.SeedWork.Auditing.Contracts;

namespace Twinkle.SeedWork.Interceptors;

public sealed class UpdateAuditableEntriesInterceptor : SaveChangesInterceptor
{
    
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        var dbContext = eventData.Context;
        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        
        var deletedEntries = dbContext.ChangeTracker.Entries<ISoftDelete>();
        foreach (var entityEntry in deletedEntries)
        {
            if (entityEntry.State != EntityState.Deleted) continue;
            entityEntry.Property(a => a.IsDeleted).CurrentValue = true;
            entityEntry.State = EntityState.Modified;
        }
        
        var createdEntries = dbContext.ChangeTracker.Entries<IHasCreationTime>();
        foreach (var entityEntry in createdEntries)
        {
            if (entityEntry.State != EntityState.Added) continue;
            entityEntry.Property(a => a.CreationTime).CurrentValue = DateTime.Now;
        }
        
        var modifiedEntries = dbContext.ChangeTracker.Entries<IHasModificationTime>();
        foreach (var entityEntry in modifiedEntries)
        {
            if (entityEntry.State != EntityState.Modified) continue;
            entityEntry.Property(a => a.ModificationTime).CurrentValue = DateTime.Now;
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}