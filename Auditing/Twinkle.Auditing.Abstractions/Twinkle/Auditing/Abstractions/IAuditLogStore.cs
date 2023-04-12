namespace Twinkle.Auditing.Abstractions;

public interface IAuditLogStore
{
    Task SaveAsync(AuditLogData auditLogData);
}