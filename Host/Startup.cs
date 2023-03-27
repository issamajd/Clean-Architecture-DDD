using System.IdentityModel.Tokens.Jwt;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DDD.Authorization.Hosting;
using DDD.Core.Application;
using DDD.Core.Hosting;
using DDD.Core.Hosting.Services;
using DDD.Identity.AppUsers;
using DDD.Identity.Module;
using DDD.Infrastructure.EfCore;
using DDD.PermissionManagement.Module;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

namespace DDD.Host;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public virtual IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>();
        services.AddIdentity<AppUser, IdentityRole<Guid>>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>();

        ConfigureAuth(services);

        services.AddControllers();
        services.AddHttpContextAccessor();

        services.AddEndpointsApiExplorer();
        ConfigureSwagger(services);
        //autofac container
        var container = new ContainerBuilder();
        container.Populate(services);

        container.AddAuthorizationCoreServices();
        container.AddPermissionProviders();
        container.AddEfCoreUnitOfWork<AppDbContext>();
        container.AddEfCoreRepositories<AppDbContext>();
        container.RegisterModule<IdentityModule>();
        container.RegisterModule<PermissionManagementModule>();

        return new AutofacServiceProvider(container.Build());
    }

    public void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo() { Title = "Demo API", Version = "v1" });

            var authorizationUrl = new Uri($"{Configuration["AuthServer:Authority"]}/connect/authorize");
            var tokenUrl = new Uri($"{Configuration["AuthServer:Authority"]}/connect/token");

            option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.OAuth2,

                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = authorizationUrl,
                        TokenUrl = tokenUrl,
                        Scopes = new Dictionary<string, string>
                        {
                            { "identity", "" },
                            { "openid", "" },
                        }
                    }
                },
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }

    public void ConfigureAuth(IServiceCollection services)
    {
        // prevent default mapping from "sub" to "nameidentitfier"
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
        services.AddScoped<IIdentityService<Guid>, IdentityService>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.Authority = Configuration["AuthServer:Authority"];
            o.RequireHttpsMetadata = Convert.ToBoolean(Configuration["AuthServer:RequireHttpsMetadata"]);
            o.Audience = "IdentityService";
        });

        services.AddAuthorization();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.OAuthClientId(Configuration["AuthServer:SwaggerClientId"]);
                setup.OAuthUsePkce();
            });
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseUnitOfWork();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapControllers();
            endpoints.MapGet("/", () => Results.Redirect("~/swagger")).ExcludeFromDescription();
        });
    }
}