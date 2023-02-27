using System.ComponentModel.DataAnnotations;

namespace DDD.Identity.ViewModels;

public class AuthorizeViewModel
{
    [Display(Name = "Application")]
    public string? ApplicationName { get; set; }
    [Display(Name = "Scope")]
    public string? Scope { get; set; }
}