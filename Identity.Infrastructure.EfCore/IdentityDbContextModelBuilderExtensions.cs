using DDD.Core.Utils;
using DDD.Identity.Customers;
using DDD.Identity.Providers;
using Microsoft.EntityFrameworkCore;

namespace DDD.Identity;

public static class IdentityDbContextModelBuilderExtensions
{
    public static void ConfigureIdentity(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

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