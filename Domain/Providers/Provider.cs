using DDD.AppUsers;

namespace DDD.Providers;

public sealed class Provider : AppUser
{
    public string? BusinessName { get; set; }

    private Provider()
    {
    }

    public Provider(Guid id, string email, string username, string? businessName = null) : base(id, email, username)
    {
        BusinessName = businessName;
    }
}