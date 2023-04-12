using System.Diagnostics;

namespace Twinkle.Auditing.Abstractions;

public class AuditingManager : IAuditingManager
{
    private readonly IAuditLogStore _auditLogStore;
    public AuditLogData? Current { get; private set; }

    public AuditingManager(IAuditLogStore auditLogStore)
    {
        _auditLogStore = auditLogStore;
    }

    public IAuditLogSaveHandle BeginAuditing()
    {
        Current = new AuditLogData();
        
        return new SaveHandle(this, Stopwatch.StartNew());
    }

    protected virtual Task SaveAsync(SaveHandle saveHandle)
    {
        saveHandle.Stopwatch.Stop();
        Current!.ExecutionDuration = Convert.ToInt32(saveHandle.Stopwatch.Elapsed.TotalMilliseconds);
        return _auditLogStore.SaveAsync(Current);
    }

    protected class SaveHandle : IAuditLogSaveHandle
    {
        private readonly AuditingManager _auditingManager;
        public readonly Stopwatch Stopwatch;

        public SaveHandle(AuditingManager auditingManager, Stopwatch stopwatch)
        {
            _auditingManager = auditingManager;
            Stopwatch = stopwatch;
        }

        public Task SaveAsync()
        {
            return _auditingManager.SaveAsync(this);
        }
    } 
}