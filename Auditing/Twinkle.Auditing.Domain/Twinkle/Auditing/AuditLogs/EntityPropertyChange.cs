using System.ComponentModel.DataAnnotations;
using Twinkle.SeedWork;

namespace Twinkle.Auditing.AuditLogs;

public class EntityPropertyChange : Entity<Guid>
{
    public Guid EntityChangeId { get; private set; }

    [MaxLength(EntityPropertyChangeConsts.MaxOldValueLength)]
    public string? OldValue { get; private set; }

    [MaxLength(EntityPropertyChangeConsts.MaxNewValueLength)]
    public string NewValue { get; private set; }

    [MaxLength(EntityPropertyChangeConsts.MaxPropertyNameLength)]
    public string PropertyName { get; private set; }

    [MaxLength(EntityPropertyChangeConsts.MaxPropertyTypeNameLength)]
    public string PropertyType { get; private set; }

    internal EntityPropertyChange(Guid id, Guid entityChangeId, string newValue, string propertyName,
        string propertyType,
        string? oldValue = null) : base(id)
    {
        EntityChangeId = Check.NotNull(entityChangeId, nameof(entityChangeId));
        NewValue = Check.NotNull(newValue, nameof(newValue));
        PropertyName = Check.NotNullOrEmpty(propertyName, nameof(propertyName));
        PropertyType = Check.NotNullOrEmpty(propertyType, nameof(propertyType));
        OldValue = oldValue;
    }
}