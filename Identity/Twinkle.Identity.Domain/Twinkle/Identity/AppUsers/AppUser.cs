using Microsoft.AspNetCore.Identity;
using Twinkle.SeedWork;

namespace Twinkle.Identity.AppUsers;

public sealed class AppUser : IdentityUser<Guid>
{
    private AppUser()
    {
    }

    public AppUser(Guid id, string username, string email)
    {
        Id = id;
        UserName = Check.NotNullOrEmpty(username, nameof(username));
        Email = Check.NotNullOrEmpty(email, nameof(email));
    }
}