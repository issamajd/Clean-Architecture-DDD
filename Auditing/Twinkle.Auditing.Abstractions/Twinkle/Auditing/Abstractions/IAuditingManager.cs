namespace Twinkle.Auditing.Abstractions;

public interface IAuditingManager
{
    public AuditLogData? Current { get; }

    public IAuditLogSaveHandle BeginAuditing();
}