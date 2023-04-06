using Microsoft.EntityFrameworkCore;
using Twinkle.Identity.Customers;
using Twinkle.Identity.Providers;
using Twinkle.SeedWork;

namespace Twinkle.Identity;

public interface IIdentityDbContext : IDbContext
{
    public DbSet<Provider> Providers { get; set; }
    public DbSet<Customer> Customers { get; set; }
}