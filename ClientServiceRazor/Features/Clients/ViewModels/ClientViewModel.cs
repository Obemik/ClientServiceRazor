using System.ComponentModel.DataAnnotations;
using ClientServiceRazor.Features.Clients.Models;

namespace ClientServiceRazor.Features.Clients.ViewModels;

public class ClientViewModel
{
    
    [Required] 
    [StringLength(50)]
    public string Surname { get; set; } = null!;
    [StringLength(50)]
    public string FirstName { get; set; } = null!;
    [StringLength(50)]
    public string? Patronymic { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [DataType(DataType.Date)]
    public DateOnly BirthDate { get; set; }
}