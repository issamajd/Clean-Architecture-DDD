using DDD.AppIdentity;
using DDD.Customers;
using DDD.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DDD.EfCore;

public class AppDbContext : IdentityDbContext<AppIdentityUser, IdentityRole<Guid>, Guid>
{
#pragma warning disable CS8618
    public DbSet<Provider> Providers { get; set; }
    public DbSet<Customer> Customers { get; set; }
#pragma warning restore CS8618
   
    public AppDbContext(DbContextOptions options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Provider>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.UserId);
        });
        builder.Entity<Customer>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.UserId);
        });
    }
}