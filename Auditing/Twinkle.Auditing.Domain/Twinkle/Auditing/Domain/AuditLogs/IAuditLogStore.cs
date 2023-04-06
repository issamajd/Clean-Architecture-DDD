using System.Linq.Expressions;

namespace Twinkle.Auditing.Domain.AuditLogs;

public interface IAuditLogStore
{
    Task<AuditLog> GetAsync(
        Expression<Func<AuditLog, bool>> predicate,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);
    Task<AuditLog> AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default);
    AuditLog Update(AuditLog entity);

}