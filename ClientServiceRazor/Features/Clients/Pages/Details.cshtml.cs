using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using ClientServiceRazor.Features.Clients.ViewModels;
using ClientServiceRazor.Features.Users.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClientServiceRazor.Features.Clients.Pages;

public class Details : PageModel
{
    private readonly AppDbContext _dbContext;
    public Details(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [BindProperty] 
    public ClientViewModel NewClient { get; set; } = new();
    
    [BindProperty(Name = "PhoneForm")] 
    public PhoneViewModel NewPhone { get; set; } = new();
    
    public List<Phone> Phones { get; set; } = new();
    
    [BindProperty]
    public bool ShowPhoneForm { get; set; }
    
    public void OnGet(int id)
    {  
        LoadData(id);
    }

    private void LoadData(int id)
    {
        NewClient = _dbContext.Clients.Where(c => c.Id == id).Select(c => new ClientViewModel
        {
            Surname = c.Surname,
            FirstName = c.FirstName,
            Patronymic = c.Patronymic,
            Email = c.Email,
            BirthDate = c.BirthDate,
        }).FirstOrDefault() ?? new ClientViewModel();
        
        Phones = _dbContext.Phones
            .Where(phone => phone.ClientId == id)
            .ToList();
    }
    
    public IActionResult OnPostShowPhoneForm(int id)
    {
        LoadData(id);
        ShowPhoneForm = true;
        return Page();
    }

    public IActionResult OnPostAddPhone(int id)
    {
        Console.WriteLine("OnPostAddPhone ");
        ModelState.Remove("Email");
        ModelState.Remove("BirthDate");
        ModelState.Remove("Surname");
        ModelState.Remove("FirstName");
        ModelState.Remove("Patronymic");
        var ms = ModelState;
        var nc = NewClient;
        var np = NewPhone;
        var tv = TryValidateModel(NewClient);
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Not valid");
            OnPost(id);
            ShowPhoneForm = true;
            return Page();
        }
        var client = _dbContext.Clients
            .Include(c => c.Phones)
            .FirstOrDefault(c => c.Id == id);
        if (client == null)
        {
            return NotFound();
        }
        var phone = new Phone
        {
            Number = NewPhone.Number,
            CountryCode = NewPhone.CountryCode,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ClientId = client.Id
        };
        _dbContext.Phones.Add(phone);
        _dbContext.SaveChanges();
        return RedirectToPage("./Details", new { id });
    }
    
    public IActionResult OnPost(int id)
    {
        if (!ModelState.IsValid)
        {
            LoadData(id);
            return Page();
        }

        var client = _dbContext.Clients.FirstOrDefault(c => c.Id == id);
        if (client == null)
        {
            return NotFound();
        }

        client.Surname = NewClient.Surname;
        client.FirstName = NewClient.FirstName;
        client.Patronymic = NewClient.Patronymic;
        client.Email = NewClient.Email;
        client.BirthDate = NewClient.BirthDate;
        client.UpdatedAt = DateTime.UtcNow;

        _dbContext.SaveChanges();

        return RedirectToPage("./Details", new { id = client.Id });
    }
    
}