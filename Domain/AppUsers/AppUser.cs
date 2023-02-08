using Microsoft.AspNetCore.Identity;

namespace DDD.AppUsers;

public abstract class AppUser : IdentityUser<Guid>
{
    public string? SocialNumber { get; set; }
    protected AppUser()
    {
    }

    protected AppUser(Guid id, string email, string username)
    {
        Id = id;
        Email = email;
        UserName = username;
    }
}