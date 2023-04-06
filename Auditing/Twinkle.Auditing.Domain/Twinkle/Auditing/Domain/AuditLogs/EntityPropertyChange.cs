using System.ComponentModel.DataAnnotations;
using Twinkle.SeedWork.Domain;
using Twinkle.SeedWork.Utils;

namespace Twinkle.Auditing.Domain.AuditLogs;

public class EntityPropertyChange : Entity<Guid>
{
    [Required] public Guid EntityChangeId { get; private set; }
    public string? OldValue { get; private set; }
    [Required] public string NewValue { get; private set; }
    [Required] public string PropertyName { get; private set; }
    [Required] public string PropertyType { get; private set; }

    internal EntityPropertyChange(Guid id, Guid entityChangeId, string newValue, string propertyName, string propertyType,
        string? oldValue = null) : base(id)
    {
        EntityChangeId = Check.NotNull(entityChangeId, nameof(entityChangeId));
        NewValue = Check.NotNull(newValue, nameof(newValue));
        PropertyName = Check.NotNullOrEmpty(propertyName, nameof(propertyName));
        PropertyType = Check.NotNullOrEmpty(propertyType, nameof(propertyType));
        OldValue = oldValue;
    }
}