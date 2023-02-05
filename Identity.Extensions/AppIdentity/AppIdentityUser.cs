using Microsoft.AspNetCore.Identity;

namespace DDD.AppIdentity;

public class AppIdentityUser : IdentityUser<Guid>
{
    public virtual string? SocialNumber { get; set; }

    public virtual Guid? CustomerId { get; set; }
    public virtual Guid? ProviderId { get; set; }
}