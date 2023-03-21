using DDD.Identity.AppUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DDD.Infrastructure.EfCore;

public static class AppDbContextExtensions
{
    internal static void SeedUsersData(this ModelBuilder builder)
    {
        builder.Entity<IdentityRole<Guid>>().HasData(
            new
            {
                Id = Guid.NewGuid(),
                Name = "CUSTOMER",
                NormalizedName = "Customer"
            },
            new
            {
                Id = Guid.NewGuid(),
                Name = "PROVIDER",
                NormalizedName = "Provider"
            },
            new
            {
                Id = Guid.NewGuid(),
                Name = "ADMIN",
                NormalizedName = "Admin"
            });
        /*
         
        #region Customer seed data
        var cusHasher = new PasswordHasher<Customer>();
        var cus1 = new Customer(Guid.NewGuid(), "customertest1@gmail.co", "test1", 19);
        cus1.PasswordHash = cusHasher.HashPassword(cus1, "test");
        var cus2 = new Customer(Guid.NewGuid(), "customertest2@gmail.co", "test2", 20);
        cus2.PasswordHash = cusHasher.HashPassword(cus2, "test");
        builder.Entity<Customer>().HasData(cus1, cus2);
        
        #endregion
        
        #region Provider seed data
        
        var provHasher = new PasswordHasher<Provider>();
        var prov1 = new Provider(Guid.NewGuid(), "providertest1@gmail.co", "test1", "Pros");
        prov1.PasswordHash = provHasher.HashPassword(prov1, "test");
        var prov2 = new Provider(Guid.NewGuid(), "providertest2@gmail.co", "test2", "Noobs");
        prov2.PasswordHash = provHasher.HashPassword(prov2, "test");
        
        builder.Entity<Provider>().HasData(prov1, prov2);
        #endregion                        
        */

        #region User Identity seed data

        var hasher = new PasswordHasher<AppUser>();
        var admin = new AppUser(Guid.NewGuid(), "admin", "admin@gmail.com");
        admin.PasswordHash = hasher.HashPassword(admin, "test");
        builder.Entity<AppUser>().HasData(admin);

        #endregion
    }
}