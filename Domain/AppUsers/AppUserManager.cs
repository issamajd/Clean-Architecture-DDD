using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DDD.AppUsers;

public class AppUserManager : UserManager<AppUser>
{
    public AppUserManager(IUserStore<AppUser> store, IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<AppUser> passwordHasher, IEnumerable<IUserValidator<AppUser>> userValidators,
        IEnumerable<IPasswordValidator<AppUser>> passwordValidators, ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<AppUser>> logger) : base(store,
        optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }

    public async Task<IdentityResult> AddUserWithRolesAsync(AppUser user,
        string password,
        IEnumerable<string> roles)
    {
        //Create user
        var createResult = await CreateAsync(user, password);
        if (!createResult.Succeeded)
            return createResult;

        var userRoleStore = Store as IUserRoleStore<AppUser> ??
                            throw new NotSupportedException("Store not supported");
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (roles == null)
        {
            throw new ArgumentNullException(nameof(roles));
        }

        //Add roles to the user
        foreach (var role in roles.Distinct())
        {
            var normalizedRole = NormalizeName(role);
            if (await userRoleStore.IsInRoleAsync(user, normalizedRole, CancellationToken).ConfigureAwait(false))
            {
                return IdentityResult.Failed(ErrorDescriber.UserAlreadyInRole(role));
            }

            await userRoleStore.AddToRoleAsync(user, normalizedRole, CancellationToken).ConfigureAwait(false);
        }

        return IdentityResult.Success;
    }
}