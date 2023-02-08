using DDD.AppUsers;
using DDD.Customers;
using DDD.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        base.OnModelCreating(builder);
        builder.Entity<IdentityUser>().UseTptMappingStrategy();
        builder.UseOpenIddict();
        builder.SeedUsersData();
    }
}