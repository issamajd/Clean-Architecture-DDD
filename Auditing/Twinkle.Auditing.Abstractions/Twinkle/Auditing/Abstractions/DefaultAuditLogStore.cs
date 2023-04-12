using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Twinkle.Auditing.Abstractions;

public class DefaultAuditLogStore : IAuditLogStore
{
    public ILogger<DefaultAuditLogStore> Logger { get; }
    
    public DefaultAuditLogStore(ILogger<DefaultAuditLogStore>? logger = null)
    {
        Logger = logger ?? NullLogger<DefaultAuditLogStore>.Instance;
    }

    public Task SaveAsync(AuditLogData auditLogData)
    {
        Logger.LogInformation(auditLogData.ToString());
        return Task.FromResult(0);
    }
}