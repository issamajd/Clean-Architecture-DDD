using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DDD.ViewModels;

public class LoginViewModel
{
    [Required] public string Username { get; set; } = null!;

    [Required] 
    [PasswordPropertyText] 
    public string Password { get; set; } = null!;

    public string? ReturnUrl { get; set; }
}