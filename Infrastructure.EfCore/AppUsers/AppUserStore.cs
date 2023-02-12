using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DDD.AppUsers;

public class AppUserStore : UserStore<AppUser, IdentityRole<Guid>, AppDbContext, Guid>
{
    public AppUserStore(AppDbContext appDbContext) : base(appDbContext)
    {
        AutoSaveChanges = false;
    }
}