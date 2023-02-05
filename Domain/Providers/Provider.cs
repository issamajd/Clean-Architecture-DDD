namespace DDD.Providers;

public class Provider
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public string? BusinessName { get; set; }

    private Provider()
    {
    }

    public Provider(Guid id, Guid userId, string? businessName = null)
    {
        Id = id;
        UserId = userId;
        BusinessName = businessName;
    }
}