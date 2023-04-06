namespace Twinkle.SeedWork.Domain;

public abstract class AggregateRoot<TKey> : Entity<TKey>
{
    protected AggregateRoot(){}
    protected AggregateRoot(TKey id) : base(id)
    {
    }
}