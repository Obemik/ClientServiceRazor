using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientServiceRazor.Features.Users.Models;

public class Role
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    [Display(Name = "Role Name")]
    [DataType(DataType.Text)]
    public string Name { get; set; } = null!;

    public ICollection<User> Users { get; set; } = new List<User>();
}