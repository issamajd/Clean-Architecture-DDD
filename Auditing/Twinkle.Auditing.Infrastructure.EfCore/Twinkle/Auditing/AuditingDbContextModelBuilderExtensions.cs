using Microsoft.EntityFrameworkCore;
using Twinkle.Auditing.AuditLogs;

namespace Twinkle.Auditing;

public static class AuditingDbContextModelBuilderExtensions
{
    public static void ConfigureAuditing(
        this ModelBuilder builder)
    {
        builder.Entity<AuditLog>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.UserId);
            
            b.HasOne<AuditLogAction>(x => x.AuditLogAction).WithOne().HasForeignKey<AuditLogAction>(x => x.AuditLogId).IsRequired();
            b.HasMany(a => a.EntityChanges).WithOne().HasForeignKey(x => x.AuditLogId).IsRequired();
        });
        builder.Entity<AuditLogAction>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.AuditLogId);
        });
        builder.Entity<EntityChange>(b =>
        {
            b.HasKey(x => x.Id);
            
            b.HasMany(a => a.EntityPropertyChanges).WithOne().HasForeignKey(x => x.EntityChangeId).IsRequired();
            b.HasIndex(x => x.AuditLogId);
        });
        builder.Entity<EntityPropertyChange>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.EntityChangeId);
        });
    }
}