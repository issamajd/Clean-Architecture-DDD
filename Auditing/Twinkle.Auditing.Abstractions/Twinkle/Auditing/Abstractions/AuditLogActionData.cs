namespace Twinkle.Auditing.Abstractions;

public class AuditLogActionData
{
    public string? ControllerName { get; set; }
    public string? ActionName { get; set; }
    public string? Parameters { get; set; }
}