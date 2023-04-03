namespace DDD.Core.Domain;

public abstract class AggregateRoot<TKey> : Entity<TKey>
{
    public override bool Equals(object? obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (AggregateRoot<TKey>)obj;
        return Id != null && Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id != null ? Id.GetHashCode() : base.GetHashCode();
    }
}