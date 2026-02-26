using System.ComponentModel.DataAnnotations;
using ClientServiceRazor.Features.Clients.Models;

namespace ClientServiceRazor.Features.Users.ViewModels;

public class PhoneViewModel
{
    [Required]
    [StringLength(50)]
    public string Number { get; set; } = string.Empty;
    [Required]
    public CountryCode CountryCode { get; set; }
}