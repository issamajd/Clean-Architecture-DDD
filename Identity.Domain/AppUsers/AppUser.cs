using DDD.Core.Utils;
using Microsoft.AspNetCore.Identity;

namespace DDD.Identity.AppUsers;

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