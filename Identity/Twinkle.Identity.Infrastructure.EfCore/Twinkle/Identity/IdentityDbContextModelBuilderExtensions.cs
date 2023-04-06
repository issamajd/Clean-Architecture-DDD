using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Twinkle.Identity.AppUsers;
using Twinkle.Identity.Customers;
using Twinkle.Identity.Providers;

namespace Twinkle.Identity;

public static class IdentityDbContextModelBuilderExtensions
{
    public static void ConfigureIdentity(
        this ModelBuilder builder)
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
        builder.SeedIdentityData();
    }

    private static void SeedIdentityData(this ModelBuilder builder)
    {
        var adminRole = new
        {
            Id = new Guid("bf2fe5b7-13f4-47b7-ae3a-4d3ce9be18d3"),
            Name = "ADMIN",
            NormalizedName = "Admin"
        };
        builder.Entity<IdentityRole<Guid>>().HasData(
            new
            {
                Id = new Guid("6fac7246-0886-4c4e-8987-b345de5b1afa"),
                Name = "CUSTOMER",
                NormalizedName = "Customer"
            },
            new
            {
                Id = new Guid("41433f97-882c-40d7-b651-6def7246e0e1"),
                Name = "PROVIDER",
                NormalizedName = "Provider"
            }, adminRole
        );
        // var hasher = new PasswordHasher<AppUser>();
        // var admin = new AppUser(new Guid("57452b20-e7c0-4b13-97c2-6b0274df831a"), "admin", "admin@gmail.com");
        // admin.PasswordHash = hasher.HashPassword(admin, "test");
        var admin = new
        {
            Id = new Guid("57452b20-e7c0-4b13-97c2-6b0274df831a"),
            AccessFailedCount = 0,
            ConcurrencyStamp = "8b128e2e-dd99-48da-8660-e9706d065d59",
            SecurityStamp = "57452b20-e7c0-4b13-97c2-6b0274df831a",
            Email = "admin@gmail.com",
            EmailConfirmed = false,
            LockoutEnabled = false,
            PasswordHash =
                "AQAAAAIAAYagAAAAEDFcFO91vNV/b1+4ymAl4sMrUHKVi/7N48OexF/rLebGBzPcIUQ2clxf935bpZvY7w==",
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            UserName = "admin",
            NormalizedUserName = "ADMIN"
        };
        builder.Entity<AppUser>().HasData(admin);
        
        builder.Entity<IdentityUserRole<Guid>>().HasData(
            new
            {
                UserId = admin.Id,
                RoleId = adminRole.Id
            });
    }
}