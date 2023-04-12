using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Twinkle.Auditing.Abstractions;
using Twinkle.SeedWork;

namespace Twinkle.Auditing.AuditLogs;

public class AuditLog : AggregateRoot<Guid>
{
    [MaxLength(AuditLogConsts.MaxApplicationNameLength)]
    public string ApplicationName { get; private set; }

    public Guid? UserId { get; private set; }
    public long ExecutionDuration { get; internal set; }
    public DateTime ExecutionTime { get; private set; }
    
    [MaxLength(AuditLogConsts.MaxClientIpAddressLength)]
    public string ClientIpAddress { get; private set; }

    [MaxLength(AuditLogConsts.MaxBrowserInfoLength)]
    public string? BrowserInfo { get; private set; }
    
    [MaxLength(AuditLogConsts.MaxHttpMethodLength)]
    public string HttpMethod { get; private set; }

    public int? StatusCode { get; internal set; }
    
    [MaxLength(AuditLogConsts.MaxUrlLength)]
    public string Url { get; private set; }

    public AuditLogAction AuditLogAction { get; private set; }
    public ICollection<EntityChange> EntityChanges { get; }

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
        UserId = userId;
        BrowserInfo = browserInfo;
        EntityChanges = new Collection<EntityChange>();
    }

    public EntityChange AddEntityChange(DateTime changeTime, EntityChangeType changeType, Guid entityId,
        string entityType)
    {
        var entityChange = new EntityChange(Guid.NewGuid(), Id, changeTime, changeType, entityId, entityType);
        EntityChanges.Add(entityChange);
        return entityChange;
    }

    public void SetAuditLogAction(string? controllerName = null, string? methodName = null, string? parameters = null)
    {
        AuditLogAction = new AuditLogAction(Guid.NewGuid(), Id, controllerName, methodName, parameters);
    }
}