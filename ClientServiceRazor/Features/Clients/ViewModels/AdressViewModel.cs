using System.ComponentModel.DataAnnotations;

namespace ClientServiceRazor.Features.Clients.ViewModels;

public class AddressViewModel
{
    [Required]
    [StringLength(100)]
    public string Country { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Region { get; set; } = null!;

    [StringLength(100)]
    public string? Area { get; set; }

    [Required]
    [StringLength(100)]
    public string City { get; set; } = null!;

    [Required]
    [StringLength(150)]
    public string Street { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string Building { get; set; } = null!;

    [StringLength(20)]
    public string? Apartment { get; set; }

    [StringLength(10)]
    public string? Entrance { get; set; }

    [StringLength(20)]
    public string? Room { get; set; }
}