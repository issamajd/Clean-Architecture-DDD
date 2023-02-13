using DDD;
using Microsoft.AspNetCore.Authentication.Cookies;
using OpenIddict.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, 
        options => { options.LoginPath = "/account/login"; });


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


        // Encryption and signing of tokens
        options
            .AddEphemeralEncryptionKey()
            .AddEphemeralSigningKey()
            .DisableAccessTokenEncryption();

        options.RegisterScopes(
            OpenIddictConstants.Scopes.OpenId,
            OpenIddictConstants.Scopes.Email,
            OpenIddictConstants.Scopes.Profile,
            OpenIddictConstants.Scopes.Phone,
            OpenIddictConstants.Scopes.Roles,
            OpenIddictConstants.Scopes.Address,
            OpenIddictConstants.Scopes.OfflineAccess
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
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(options =>
{
    options.MapControllers();
    options.MapDefaultControllerRoute();
});

app.Run();