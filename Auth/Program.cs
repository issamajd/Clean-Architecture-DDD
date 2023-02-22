using DDD;
using DDD.AppUsers;
using DDD.Customers;
using DDD.Providers;
using DDD.SeedWork;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>()
    .AddUserStore<AppUserStore>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ??
                       throw new InvalidOperationException("Google ClientId not defined");
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ??
                           throw new InvalidOperationException("Google ClientSecret not defined");
});
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy  =>
        {
            //TODO change hardcoded origins
            policy.WithOrigins("https://localhost:7206",
                    "http://localhost:5220")
                .WithMethods("POST", "GET", "OPTIONS")
                .WithHeaders("Authorization", "Content-Type", "Accept", "Origin", "x-requested-with")

                .SetIsOriginAllowedToAllowWildcardSubdomains();
        });
});

builder.Services.AddOpenIddict()
    .AddCore(options => { options.UseEntityFrameworkCore().UseDbContext<AppDbContext>(); })
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

builder.Services.AddControllersWithViews();
builder.Services.AddHostedService<Worker>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
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

app.Run();