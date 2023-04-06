namespace Twinkle.SeedWork.Domain;
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
    public override bool Equals(object? obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Entity<TKey>)obj;
        return Id != null && Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id != null ? Id.GetHashCode() : base.GetHashCode();
    }
}