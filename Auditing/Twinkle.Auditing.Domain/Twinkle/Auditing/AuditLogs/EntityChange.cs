using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Twinkle.Auditing.Abstractions;
using Twinkle.SeedWork;

namespace Twinkle.Auditing.AuditLogs;

public class EntityChange : Entity<Guid>
{
    public Guid AuditLogId { get; private set; }
    public DateTime ChangeTime { get; private set; }
    public EntityChangeType ChangeType { get; private set; }
    public Guid EntityId { get; private set; }

    [MaxLength(EntityChangeConsts.MaxEntityTypeNameLength)]
    public string EntityType { get; private set; }

    public ICollection<EntityPropertyChange> EntityPropertyChanges { get; }

    private EntityChange()
    {
    }

    internal EntityChange(Guid id, Guid auditLogId, DateTime changeTime, EntityChangeType changeType, Guid entityId,
        string entityType) : base(id)
    {
        AuditLogId = Check.NotNull(auditLogId, nameof(auditLogId));
        ChangeTime = Check.NotNull(changeTime, nameof(changeTime));
        ChangeType = Check.NotNull(changeType, nameof(changeType));
        EntityId = Check.NotNull(entityId, nameof(entityId));
        EntityType = Check.NotNullOrEmpty(entityType, nameof(entityType));

        EntityPropertyChanges = new Collection<EntityPropertyChange>();
    }

    public void AddEntityPropertyChange(string newValue, string propertyName,
        string propertyType,
        string? oldValue = null)
    {
        var entityPropertyChange = new EntityPropertyChange(Guid.NewGuid(), Id, newValue, propertyName,
            propertyType, oldValue);
        EntityPropertyChanges.Add(entityPropertyChange);
    }
}