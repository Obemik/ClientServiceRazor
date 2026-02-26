using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClientServiceRazor.Features.Clients.Pages;

public class AddressPage : PageModel
{
    private readonly AppDbContext _dbContext;

    public AddressPage(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [BindProperty]
    public AddressViewModel AddressModel { get; set; } = new();

    public string ClientFullName { get; set; } = string.Empty;
    public bool AddressExists { get; set; }

    public void OnGet(int id)
    {
        LoadData(id);
    }

    private void LoadData(int id)
    {
        var client = _dbContext.Clients
            .Include(c => c.Address)
            .FirstOrDefault(c => c.Id == id);

        if (client != null)
        {
            ClientFullName = $"{client.Surname} {client.FirstName} {client.Patronymic}".Trim();

            if (client.Address != null)
            {
                AddressExists = true;
                AddressModel = new AddressViewModel
                {
                    Country = client.Address.Country,
                    Region = client.Address.Region,
                    Area = client.Address.Area,
                    City = client.Address.City,
                    Street = client.Address.Street,
                    Building = client.Address.Building,
                    Apartment = client.Address.Apartment,
                    Entrance = client.Address.Entrance,
                    Room = client.Address.Room
                };
            }
        }
    }

    public IActionResult OnPost(int id)
    {
        if (!ModelState.IsValid)
        {
            LoadData(id);
            return Page();
        }

        var client = _dbContext.Clients
            .Include(c => c.Address)
            .FirstOrDefault(c => c.Id == id);

        if (client == null)
        {
            return NotFound();
        }

        if (client.Address == null)
        {
            client.Address = new Models.Address
            {
                Country = AddressModel.Country,
                Region = AddressModel.Region,
                Area = AddressModel.Area,
                City = AddressModel.City,
                Street = AddressModel.Street,
                Building = AddressModel.Building,
                Apartment = AddressModel.Apartment,
                Entrance = AddressModel.Entrance,
                Room = AddressModel.Room,
                ClientId = id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _dbContext.Addresses.Add(client.Address);
        }
        else
        {
            client.Address.Country = AddressModel.Country;
            client.Address.Region = AddressModel.Region;
            client.Address.Area = AddressModel.Area;
            client.Address.City = AddressModel.City;
            client.Address.Street = AddressModel.Street;
            client.Address.Building = AddressModel.Building;
            client.Address.Apartment = AddressModel.Apartment;
            client.Address.Entrance = AddressModel.Entrance;
            client.Address.Room = AddressModel.Room;
            client.Address.UpdatedAt = DateTime.UtcNow;
        }

        _dbContext.SaveChanges();
        return RedirectToPage(new { id });
    }

    public IActionResult OnPostDelete(int id)
    {
        var address = _dbContext.Addresses.FirstOrDefault(a => a.ClientId == id);
        if (address != null)
        {
            _dbContext.Addresses.Remove(address);
            _dbContext.SaveChanges();
        }

        return RedirectToPage(new { id });
    }
}