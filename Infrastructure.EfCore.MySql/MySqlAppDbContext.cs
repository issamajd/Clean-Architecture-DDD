using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DDD;

public class MySqlAppDbContext : AppDbContext
{
    private readonly IConfiguration _configuration;

    public MySqlAppDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("Default");
        optionsBuilder.UseMySQL(connectionString ?? throw new InvalidOperationException("Unable to connect to DB"));
    }
}