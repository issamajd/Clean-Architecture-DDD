using Microsoft.AspNetCore.Identity;

namespace DDD.AppIdentity;

public static class UserManagerExtensions
{
    public static AppIdentityUser? FindCustomer(this UserManager<AppIdentityUser> userManager, Guid id)
    {
        return userManager?.Users?.Where(user => user.CustomerId == id).FirstOrDefault();
    }
    
    public static AppIdentityUser? FindProvider(this UserManager<AppIdentityUser> userManager, Guid id)
    {
        return userManager?.Users?.Where(user => user.ProviderId == id).FirstOrDefault();
    }
}