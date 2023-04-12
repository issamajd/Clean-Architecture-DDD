namespace Twinkle.SeedWork.Auditing;

public interface IAuditLogStore
{
    Task SaveAsync(AuditLogData auditLogData);
}