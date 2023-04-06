using System.ComponentModel.DataAnnotations;
using Twinkle.SeedWork;

namespace Twinkle.Auditing.AuditLogs;

public class AuditLogAction : Entity<Guid>
{
    [Required] public Guid AuditLogId { get; private set; }
    [Required] public string ControllerName { get; private set; }
    [Required] public string MethodName { get; private set; }
    [Required] public string Parameters { get; private set; }
    [Required] public DateTime ExecutionTime { get; private set; }
    public ulong ExecutionDuration { get; internal set; }

    private AuditLogAction()
    {
    }
    
    internal AuditLogAction(Guid id,Guid auditLogId, string controllerName, string methodName, string parameters,
        DateTime executionTime) : base(id)
    {
        ControllerName = Check.NotNullOrEmpty(controllerName,nameof(controllerName));
        AuditLogId = Check.NotNull(auditLogId,nameof(auditLogId));
        MethodName = Check.NotNullOrEmpty(methodName,nameof(methodName));
        Parameters = Check.NotNullOrEmpty(parameters,nameof(parameters));
        ExecutionTime = Check.NotNull(executionTime,nameof(executionTime));
    }
}