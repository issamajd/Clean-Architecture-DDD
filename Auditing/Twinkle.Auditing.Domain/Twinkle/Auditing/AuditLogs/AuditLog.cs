using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Twinkle.SeedWork;

namespace Twinkle.Auditing.AuditLogs;

public class AuditLog : AggregateRoot<Guid>
{
    [Required] public string ApplicationName { get; private set; }
    public Guid UserId { get; private set; }
    public ulong ExecutionDuration { get; internal set; }
    [Required] public DateTime ExecutionTime { get; private set; }
    [Required] public string ClientIpAddress { get; private set; }
    public string? BrowserInfo { get; private set; }
    [Required] public string HttpMethod { get; private set; }
    public int? StatusCode { get; internal set; }
    [Required] public string Url { get; private set; }

    public AuditLogAction AuditLogAction { get; private set; }
    private ICollection<EntityChange> EntityChanges { get; }

    private AuditLog()
    {
    }

    public AuditLog(Guid id, string applicationName, DateTime executionTime, string clientIpAddress, string url,
        string httpMethod, Guid? userId, string? browserInfo = null) : base(id)
    {
        ApplicationName = Check.NotNull(applicationName, nameof(applicationName));
        ExecutionTime = Check.NotNull(executionTime, nameof(executionTime));
        ClientIpAddress = Check.NotNullOrEmpty(clientIpAddress, nameof(clientIpAddress));
        if (!IPAddress.TryParse(ClientIpAddress, out var address))
            throw new ArgumentException($"{ClientIpAddress} is not a valid ip address!", nameof(ClientIpAddress));
        Url = Check.NotNullOrEmpty(url, nameof(url));
        if (!Uri.IsWellFormedUriString(Url, UriKind.Absolute))
            throw new ArgumentException($"{Url} is not a valid URI!", nameof(Url));

        HttpMethod = Check.NotNullOrEmpty(httpMethod, nameof(httpMethod));
        BrowserInfo = browserInfo;
        EntityChanges = new Collection<EntityChange>();
    }

    public void SetExecutionInfo(ulong executionDuration, HttpStatusCode statusCode)
    {
        StatusCode = (int)statusCode;
        ExecutionDuration = executionDuration;
    }

    public EntityChange AddEntityChange(DateTime changeTime, EntityChangeType changeType, Guid entityId,
        string entityType)
    {
        var entityChange = new EntityChange(Guid.NewGuid(), Id, changeTime, changeType, entityId, entityType);
        EntityChanges.Add(entityChange);
        return entityChange;
    }

    public void AddEntityPropertyChange(EntityChange entityChange, string newValue, string propertyName,
        string propertyType,
        string? oldValue = null)
    {
        var entityPropertyChange = new EntityPropertyChange(Guid.NewGuid(), entityChange.Id, newValue, propertyName,
            propertyName, oldValue);
        entityChange.EntityPropertyChanges.Add(entityPropertyChange);
    }

    public void SetAuditLogAction(string controllerName, string methodName, string parameters,
        DateTime executionTime)
    {
        AuditLogAction = new AuditLogAction(Guid.NewGuid(), Id, controllerName, methodName, parameters,
            executionTime);
    }
}