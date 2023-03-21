using DDD.Core.Infrastructure.EfCore;
using DDD.Identity.Customers;
using DDD.Identity.Providers;
using Microsoft.EntityFrameworkCore;

namespace DDD.Identity;

public interface IIdentityDbContext : IDbContext
{
    public DbSet<Provider> Providers { get; set; }
    public DbSet<Customer> Customers { get; set; }
}