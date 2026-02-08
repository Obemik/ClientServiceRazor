using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientServiceRazor.Features.Clients.Models;

public class Client
{
    [Key] //Позначає проперті як первинний ключ
    //Генерація Id за рахунок бази
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //(AUTO_INCREMENT, IDENTITY (SERIAL))
    // [DatabaseGenerated(DatabaseGeneratedOption.None)] //Відключення автогенерації, ручне встановлення ID через код
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)] //Значення обчислюється базою (тригер, формула)
    public int Id { get; set; }
    [Required] //NOT NULL
    [MaxLength(50)]
    [MinLength(1)]
    // [StringLength(50)]
    [Display(Name = "ClientSurname")]
    [DataType(DataType.Text)]
    public string Surname { get; set; } = null!;
    [StringLength(50)]
    [DataType(DataType.Text)]
    public string FirstName { get; set; } = null!;
    [MaxLength(50)]
    [DataType(DataType.Text)]
    public string? Patronymic { get; set; }
    [MaxLength(100)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    //+
    public DateOnly BirthDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    //One Client -> Many Phones
    public ICollection<Phone> Phones { get; set; } = new List<Phone>();
    //One Client -> One Address
    public Address? Address { get; set; }
    public ICollection<ClientFinanceAccount> ClientFinanceAccounts { get; set; } = new List<ClientFinanceAccount>();
}