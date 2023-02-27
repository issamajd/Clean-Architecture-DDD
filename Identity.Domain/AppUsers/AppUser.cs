using Microsoft.AspNetCore.Identity;

namespace DDD.Identity.AppUsers;

public class AppUser : IdentityUser<Guid>
{
    
    private AppUser()
    {
    }
   
    public AppUser(Guid id, string username, string email)
    {
        //TODO Check if values are null
        Id = id;
        Email = email;
        UserName = username;
    }
}