namespace DDD.Core.Domain;

public class EntityNotFoundException : Exception
{
    /// <summary>
    /// Type of the entity.
    /// </summary>
    public Type EntityType { get; }
    /// <summary>
    /// Id of the Entity.
    /// </summary>
    public object? Id { get; }
  
    public EntityNotFoundException(Type entityType, object? id = null, Exception? innerException = null)
        : base(
            id == null ? 
                $"There is no such an entity with given id. Entity type: {entityType.FullName}" :
                $"There is no such an entity. Entity type: {entityType.FullName}, id: {id}",
            innerException
            )
    {
        EntityType = entityType;
        Id = id;
    }
}