using DDD.Identity.AppUsers;
using DDD.Identity.Customers;
using DDD.Identity.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DDD.Identity;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
#pragma warning disable CS8618
    public DbSet<Provider> Providers { get; set; }
    public DbSet<Customer> Customers { get; set; }
#pragma warning restore CS8618
   
    public AppDbContext(DbContextOptions options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.UseOpenIddict();
        builder.ConfigureAppDb();
        builder.SeedUsersData();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString =  BuildConfiguration().GetConnectionString("Default") 
                                ?? throw new InvalidOperationException("Connection string not provided");
        var serverVersion = ServerVersion.AutoDetect(connectionString);
        optionsBuilder.UseMySql(connectionString, serverVersion);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Identity.Host/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}