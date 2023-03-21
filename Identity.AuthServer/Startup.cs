using Autofac;
using Autofac.Extensions.DependencyInjection;
using DDD.Identity.AppUsers;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace DDD.Identity;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public virtual IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AuthServerDbContext>();
        services.AddIdentity<AppUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AuthServerDbContext>();

        ConfigureAuth(services);
        
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    //TODO change hardcoded origins
                    policy.WithOrigins("https://localhost:7206",
                            "http://localhost:5220")
                        .WithMethods("POST", "GET", "OPTIONS")
                        .WithHeaders("Authorization", "Content-Type", "Accept", "Origin", "x-requested-with")
                        .SetIsOriginAllowedToAllowWildcardSubdomains();
                });
        });

        services.AddControllersWithViews();
        services.AddHostedService<Worker>();

        //autofac container
        var container = new ContainerBuilder();
        container.Populate(services);
        return new AutofacServiceProvider(container.Build());
    }

    public void ConfigureAuth(IServiceCollection services)
    {
        services.AddAuthentication().AddGoogle(options =>
        {
            options.ClientId = Configuration["Authentication:Google:ClientId"] ??
                               throw new InvalidOperationException("Google ClientId not defined");
            options.ClientSecret = Configuration["Authentication:Google:ClientSecret"] ??
                                   throw new InvalidOperationException("Google ClientSecret not defined");
        });

        services.AddOpenIddict()
            .AddCore(options => { options.UseEntityFrameworkCore().UseDbContext<AuthServerDbContext>(); })
            .AddServer(options =>
            {
                options
                    .AllowAuthorizationCodeFlow()
                    .AllowClientCredentialsFlow()
                    .RequireProofKeyForCodeExchange()
                    .AllowHybridFlow()
                    .AllowImplicitFlow()
                    .AllowDeviceCodeFlow()
                    .AllowRefreshTokenFlow();


                options
                    .SetAuthorizationEndpointUris("/connect/authorize", "connect/authorize/callback")
                    .SetTokenEndpointUris("/connect/token")
                    .SetUserinfoEndpointUris("/connect/userinfo")
                    .SetDeviceEndpointUris("device")
                    .SetIntrospectionEndpointUris("connect/introspect")
                    .SetLogoutEndpointUris("connect/logout")
                    .SetRevocationEndpointUris("connect/revocat")
                    .SetVerificationEndpointUris("connect/verify");

                options
                    .DisableAccessTokenEncryption();

                options.RegisterScopes(
                    OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Scopes.Email,
                    OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Phone,
                    OpenIddictConstants.Scopes.Roles,
                    OpenIddictConstants.Scopes.Address,
                    OpenIddictConstants.Scopes.OfflineAccess,
                    "identity"
                );

                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                options
                    .UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough()
                    .EnableLogoutEndpointPassthrough()
                    .EnableVerificationEndpointPassthrough()
                    .EnableStatusCodePagesIntegration();

                //TODO check if dev environment by using pre-configuration
                options
                    .AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();
            })
            .AddValidation(options =>
            {
                options.AddAudiences("IdentityServer");
                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();
                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(options =>
        {
            options.MapControllers();
            options.MapDefaultControllerRoute();
        });
    }
}