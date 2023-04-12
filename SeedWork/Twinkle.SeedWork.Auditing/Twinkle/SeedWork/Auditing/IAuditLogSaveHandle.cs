namespace Twinkle.SeedWork.Auditing;

public interface IAuditLogSaveHandle
{
    Task SaveAsync();
}