namespace Twinkle.Auditing.Abstractions;

public interface IAuditLogSaveHandle
{
    Task SaveAsync();
}