using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientServiceRazor.Features.Clients.Models;

public class Address
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] 
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string Country { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string Region { get; set; } = null!; 

    [MaxLength(100)] 
    [Column(TypeName = "varchar(100)")]
    public string? Area { get; set; }

    [Required] 
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string City { get; set; } = null!;

    [Required] 
    [MaxLength(150)]
    [Column(TypeName = "varchar(150)")]
    public string Street { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
    public string Building { get; set; } = null!;

    [MaxLength(20)] 
    [Column(TypeName = "varchar(20)")]
    public string? Apartment { get; set; }

    [MaxLength(10)] 
    [Column(TypeName = "varchar(10)")]
    public string? Entrance { get; set; } = null!;

    [MaxLength(20)] 
    [Column(TypeName = "varchar(20)")]
    public string? Room { get; set; }

    [Column(TypeName = "timestamp with time zone")] 
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "timestamp with time zone")] 
    public DateTime UpdatedAt { get; set; }

    // One Address -> One Client
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
}