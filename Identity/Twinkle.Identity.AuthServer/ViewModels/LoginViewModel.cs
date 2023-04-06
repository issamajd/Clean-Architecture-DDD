using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace Twinkle.Identity.ViewModels;

public class LoginViewModel
{
    [Required] public string Username { get; set; } = null!;

    [Required] 
    [PasswordPropertyText] 
    public string Password { get; set; } = null!;

    public string? ReturnUrl { get; set; }
    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
    public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();
}