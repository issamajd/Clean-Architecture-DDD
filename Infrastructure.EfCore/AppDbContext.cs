using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Twinkle.Identity;
using Twinkle.Identity.AppUsers;
using Twinkle.Identity.Customers;
using Twinkle.Identity.Providers;
using Twinkle.PermissionManagement;
using Twinkle.PermissionManagement.PermissionGrants;

namespace Infrastructure.EfCore;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>,
    IIdentityDbContext,
    IPermissionManagementDbContext
{
#pragma warning disable CS8618
    public DbSet<Provider> Providers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<PermissionGrant> PermissionGrants { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
#pragma warning restore CS8618

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.UseOpenIddict();
        builder.ConfigureIdentity();
        builder.ConfigurePermissionManagement();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = BuildConfiguration().GetConnectionString("Default")
                               ?? throw new InvalidOperationException("Connection string not provided");
        var serverVersion = ServerVersion.AutoDetect(connectionString);
        optionsBuilder.UseMySql(connectionString, serverVersion);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Host/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}