using System.Collections.ObjectModel;

namespace Twinkle.SeedWork.Auditing;

public class EntityChangeData
{
    public DateTime ChangeTime { get; set; }
    public EntityChangeType ChangeType { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; }
    public ICollection<EntityPropertyChangeData> EntityPropertyChanges { get; set; }


    public EntityChangeData()
    {
        EntityPropertyChanges = new Collection<EntityPropertyChangeData>();
    }
}