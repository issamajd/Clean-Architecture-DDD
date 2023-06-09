using Microsoft.AspNetCore.Http;
using Twinkle.SeedWork.Security;

namespace Twinkle.SeedWork.AspNetCore;

public class IdentityService : IIdentityService<Guid>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityService(IHttpContextAccessor context)
    {
        _httpContextAccessor = context.HttpContext != null ? context : throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc cref="IIdentityService{TKey}.GetUserId"/>
    /// <exception cref="ArgumentNullException">If the user id couldn't be retrieved properly from the HttpContext</exception>
    /// <exception cref="FormatException">If the user id is not formatted as Guid</exception>
    public Guid GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext!.User.FindFirst("sub")?.Value;
        userId = Check.NotNullOrEmpty(userId, nameof(userId));
        return Guid.Parse(userId);
    }
}