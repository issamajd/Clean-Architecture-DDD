using Twinkle.SeedWork;

namespace Twinkle.Auditing.AuditLogs;

public class EfCoreAuditLogRepository : EfCoreRepository<AuditLog, IAuditingDbContext>, IAuditLogRepository
{
    public EfCoreAuditLogRepository(IAuditingDbContext dbContext) : base(dbContext)
    {
    }
}