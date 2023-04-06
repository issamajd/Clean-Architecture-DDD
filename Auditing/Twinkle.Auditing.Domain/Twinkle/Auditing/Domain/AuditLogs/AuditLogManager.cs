using Twinkle.SeedWork.Domain;

namespace Twinkle.Auditing.Domain.AuditLogs;

public class AuditLogManager : IDomainService
{
    private readonly IAuditLogStore _auditLogStore;

    public AuditLogManager(IAuditLogStore auditLogStore)
    {
        _auditLogStore = auditLogStore;
    }

    
}