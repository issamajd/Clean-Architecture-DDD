using Microsoft.AspNetCore.Identity;

namespace DDD.AppUsers;

public class AppUser : IdentityUser<Guid>
{
    
    private AppUser()
    {
    }
    
    public AppUser(Guid id, string username)
    {
        Id = id;
        UserName = username;
    }
}