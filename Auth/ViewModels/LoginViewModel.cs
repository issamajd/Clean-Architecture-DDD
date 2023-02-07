using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DDD.ViewModels;

public class LoginViewModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; }
    public string? ReturnUrl { get; set; }
}