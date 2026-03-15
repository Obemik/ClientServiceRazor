using System.ComponentModel.DataAnnotations;

namespace ClientServiceRazor.Features.Users.ViewModels;

public class LoginViewModel
{
    [Required, StringLength(50)]
    public string Login { get; set; } = null!;
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}