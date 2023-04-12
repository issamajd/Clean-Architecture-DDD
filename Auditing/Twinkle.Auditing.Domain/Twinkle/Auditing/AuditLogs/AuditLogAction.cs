using System.ComponentModel.DataAnnotations;
using Twinkle.SeedWork;

namespace Twinkle.Auditing.AuditLogs;

public class AuditLogAction : Entity<Guid>
{
    public Guid AuditLogId { get; private set; }

    [MaxLength(AuditLogActionConsts.MaxControllerNameLength)]
    public string? ControllerName { get; private set; }

    [MaxLength(AuditLogActionConsts.MaxMethodNameLength)]
    public string? ActionName { get; private set; }

    [MaxLength(AuditLogActionConsts.MaxParametersLength)]
    public string? Parameters { get; private set; }


    private AuditLogAction()
    {
    }

    internal AuditLogAction(Guid id, Guid auditLogId, string? controllerName = null, string? actionName = null,
        string? parameters = null)
        : base(id)
    {
        AuditLogId = auditLogId;
        ControllerName = controllerName;
        ActionName = actionName;
        Parameters = parameters;
    }
}