namespace DDD.SeedWork;
public abstract class Entity{}
public abstract class Entity<TKey> : Entity
{
    public virtual TKey Id { get; protected set; }

    protected Entity()
    {
    }

    protected Entity(TKey id)
    {
        Id = id;
    }
}