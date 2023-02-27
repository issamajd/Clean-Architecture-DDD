using System.ComponentModel.DataAnnotations;

namespace DDD.Identity.Customers;

public class RegisterCustomerAccountDto
{
    public string Username { get; set; } = null!;
    [EmailAddress]
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int? Age { get; set; }
}