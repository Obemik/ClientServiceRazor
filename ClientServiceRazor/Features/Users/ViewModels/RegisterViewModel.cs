using System.ComponentModel.DataAnnotations;

namespace ClientServiceRazor.Features.Users.ViewModels;

public class RegisterViewModel
{
    [Required, StringLength(50)]
    public string Login { get; set; } = null!;
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    [Required, DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}