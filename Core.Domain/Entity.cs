namespace DDD.Core.Domain;
public abstract class Entity{}
public abstract class Entity<TKey> : Entity
{
    public virtual TKey Id { get; protected set; } = default!; 

    protected Entity()
    {
    }

    protected Entity(TKey id)
    {
        Id = id;
    }
}