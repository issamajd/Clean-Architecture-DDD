using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Twinkle.SeedWork;

namespace Twinkle.Auditing.AuditLogs;

public class EntityChange : Entity<Guid>
{
    [Required] public Guid AuditLogId { get; private set; }
    [Required] public DateTime ChangeTime { get; private set; }
    [Required] public EntityChangeType ChangeType { get; private set; }
    [Required] public Guid EntityId { get; private set; }
    [Required] public string EntityType { get; private set; }
    internal ICollection<EntityPropertyChange> EntityPropertyChanges { get; }

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
}