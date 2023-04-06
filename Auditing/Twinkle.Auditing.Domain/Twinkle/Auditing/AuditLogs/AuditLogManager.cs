using Twinkle.SeedWork;

namespace Twinkle.Auditing.AuditLogs;

public class AuditLogManager : IDomainService
{
    private readonly IAuditLogStore _auditLogStore;

    public AuditLogManager(IAuditLogStore auditLogStore)
    {
        _auditLogStore = auditLogStore;
    }

    
}