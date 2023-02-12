using DDD.Customers;
using DDD.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DDD;

public static class AppDbContextExtensions
{
    internal static void SeedUsersData(this ModelBuilder builder)
    {
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
            
        #region User Identity seed data
       
        var hasher = new PasswordHasher<IdentityUser<Guid>>();
        var admin = new IdentityUser<Guid> {Id = Guid.NewGuid(),  Email = "providertest1@gmail.co", UserName = "admin"};
        admin.PasswordHash = hasher.HashPassword(admin, "test");
        builder.Entity<IdentityUser<Guid>>().HasData(admin);
        
        #endregion
        */
    }
    public static void ConfigureAppDb(this ModelBuilder builder)
    {
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