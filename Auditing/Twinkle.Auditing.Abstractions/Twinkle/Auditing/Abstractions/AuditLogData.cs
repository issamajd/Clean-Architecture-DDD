using System.Collections.ObjectModel;
using System.Text;

namespace Twinkle.Auditing.Abstractions;

public class AuditLogData
{
    public string ApplicationName { get; set; }
    public Guid? UserId { get; set; }
    public string ClientIpAddress { get; set; }
    public string? BrowserInfo { get; set; }
    public string HttpMethod { get; set; }
    public int? StatusCode { get; set; }
    public string Url { get; set; }

    public DateTime ExecutionTime { get; set; }
    public long ExecutionDuration { get; set; }
    public AuditLogActionData? AuditLogActionData { get; set; }
    public ICollection<EntityChangeData> EntityChanges { get; }
    public ICollection<Exception> Exceptions { get; }

    public AuditLogData()
    {
        EntityChanges = new Collection<EntityChangeData>();
        Exceptions = new Collection<Exception>();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"AUDIT LOG: [{StatusCode?.ToString() ?? "---"}: {(HttpMethod ?? "-------").PadRight(7)}] {Url}");
        sb.AppendLine($"- UserId                 : {UserId}");
        sb.AppendLine($"- ClientIpAddress        : {ClientIpAddress}");
        sb.AppendLine($"- ExecutionDuration      : {ExecutionDuration}");

        sb.AppendLine("- Action:");
        sb.AppendLine($"  - {AuditLogActionData.ControllerName}.{AuditLogActionData.ActionName}");
        sb.AppendLine($"    {AuditLogActionData.Parameters}");

        if (Exceptions.Any())
        {
            sb.AppendLine("- Exceptions:");
            foreach (var exception in Exceptions)
            {
                sb.AppendLine($"  - {exception.Message}");
                sb.AppendLine($"    {exception}");
            }
        }

        if (!EntityChanges.Any()) return sb.ToString();
        sb.AppendLine("- Entity Changes:");
        foreach (var entityChange in EntityChanges)
        {
            sb.AppendLine(
                $"  - [{entityChange.ChangeType}] {entityChange.EntityType}, Id = {entityChange.EntityId}");
            foreach (var propertyChange in entityChange.EntityPropertyChanges)
            {
                sb.AppendLine(
                    $"    {propertyChange.PropertyName}: {propertyChange.OldValue} -> {propertyChange.NewValue}");
            }
        }

        return sb.ToString();
    }
}