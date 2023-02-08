using DDD.AppUsers;
using DDD.Customers;
using DDD.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DDD;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
#pragma warning disable CS8618
    public DbSet<Provider> Providers { get; set; }
    public DbSet<Customer> Customers { get; set; }
#pragma warning restore CS8618
   
    public AppDbContext(DbContextOptions options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<AppUser>();
        // builder.Entity<AppUser>().UseTpcMappingStrategy();
        base.OnModelCreating(builder);
        builder.UseOpenIddict();
        builder.SeedUsersData();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString =  BuildConfiguration().GetConnectionString("Default");
        optionsBuilder.UseMySQL(connectionString ?? throw new InvalidOperationException("Unable to connect to DB"));
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Host/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}