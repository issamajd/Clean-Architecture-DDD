using System.ComponentModel.DataAnnotations;

namespace DDD.Providers;

public class RegisterProviderAccountDto
{
    public string Username { get; set; } = null!;
    [EmailAddress]
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    [MinLength(4)]
    public string? BusinessName { get; set; }
}