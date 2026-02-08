using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientServiceRazor.Features.Users.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    [Display(Name = "User Login")]
    [DataType(DataType.Text)]
    public string Login { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    [Column(TypeName = "varchar(255)")]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    [Display(Name = "Email Address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int StatusId { get; set; }

    public Status Status { get; set; } = null!;

    public int RoleId { get; set; }

    public Role Role { get; set; } = null!;
}