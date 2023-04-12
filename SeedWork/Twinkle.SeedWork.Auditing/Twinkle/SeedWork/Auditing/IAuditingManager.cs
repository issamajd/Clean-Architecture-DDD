namespace Twinkle.SeedWork.Auditing;

public interface IAuditingManager
{
    public AuditLogData? Current { get; }

    public IAuditLogSaveHandle BeginAuditing();
}