using Twinkle.Auditing.Abstractions;

namespace Twinkle.Auditing.AuditLogs;

public class AuditLogDataToAuditLogMapper
{
    public AuditLog GetAuditLog(AuditLogData auditLogData)
    {
        var auditLog = new AuditLog(Guid.NewGuid(), auditLogData.ApplicationName,
            auditLogData.ExecutionTime, auditLogData.ClientIpAddress, auditLogData.Url,
            auditLogData.HttpMethod, auditLogData.UserId, auditLogData.BrowserInfo)
        {
            ExecutionDuration = auditLogData.ExecutionDuration,
            StatusCode = auditLogData.StatusCode
        };

        if (auditLogData.AuditLogActionData != null)
        {
            auditLog.SetAuditLogAction(auditLogData.AuditLogActionData.ControllerName,
                auditLogData.AuditLogActionData.ActionName, auditLogData.AuditLogActionData.Parameters);
        }

        if (auditLogData.EntityChanges.Count <= 0) return auditLog;
        foreach (var entityChangeData in auditLogData.EntityChanges)
        {
            var entityChange = auditLog.AddEntityChange(entityChangeData.ChangeTime, entityChangeData.ChangeType,
                entityChangeData.EntityId, entityChangeData.EntityType);
            foreach (var entityPropertyChangeData in entityChangeData.EntityPropertyChanges)
            {
                entityChange.AddEntityPropertyChange(entityPropertyChangeData.NewValue,
                    entityPropertyChangeData.PropertyName, entityPropertyChangeData.PropertyType,
                    entityPropertyChangeData.OldValue);
            }
        }

        return auditLog;
    }
}