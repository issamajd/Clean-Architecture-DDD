using Microsoft.EntityFrameworkCore;
using Twinkle.Auditing.AuditLogs;
using Twinkle.SeedWork;

namespace Twinkle.Auditing;

public interface IAuditingDbContext : IDbContext
{
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<AuditLogAction> AuditLogActions { get; set; }
    public DbSet<EntityChange> EntityChanges { get; set; }
    public DbSet<EntityPropertyChange> EntityPropertyChanges { get; set; }
}