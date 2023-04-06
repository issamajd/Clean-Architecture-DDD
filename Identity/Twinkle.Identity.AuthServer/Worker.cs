using OpenIddict.Abstractions;

namespace Twinkle.Identity;

public class Worker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public Worker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AuthServerDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        if (await manager.FindByClientIdAsync("postman", cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "postman",
                ClientSecret = "postman-secret",
                DisplayName = "Postman",
                RedirectUris = { new Uri("https://oauth.pstmn.io/v1/callback") },
                
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Introspection,

                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Revocation,
                    //
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

                    //
                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                    OpenIddictConstants.Permissions.Prefixes.Scope + "identity"
                }
            }, cancellationToken);
            
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "AppIdentity_Swagger",
                DisplayName = "Identity Swagger",
                ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
                Type = OpenIddictConstants.ClientTypes.Public,
                RedirectUris = { new Uri("https://localhost:7206/swagger/oauth2-redirect.html") },
                
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Introspection,

                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Revocation,
                    OpenIddictConstants.Permissions.Endpoints.Logout,

                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    
                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                    
                    OpenIddictConstants.Permissions.Scopes.Address,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Roles,
                    OpenIddictConstants.Permissions.Prefixes.Scope + "identity"
                }
            }, cancellationToken);
        }
        var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        if (await scopeManager.FindByNameAsync("identity", cancellationToken) is null)
        {

            await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                DisplayName = "AppIdentity API access",
                Name = "identity",
                Resources =
                {
                    "IdentityService"
                }
            }, cancellationToken);
        }

    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}