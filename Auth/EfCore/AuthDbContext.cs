using Microsoft.EntityFrameworkCore;

namespace DDD.EfCore;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }
}