using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClientServiceRazor.Tests;

public class AddressTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateAddress()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "test@test.com",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };

        var address = new Address
        {
            Country = "Ukraine",
            Region = "Vinnytsia",
            Area = "Vinnytskyy",
            City = "Vinnytsia",
            Street = "Soborna",
            Building = "10",
            Apartment = "5",
            Entrance = "2",
            Room = "101",
            Client = client,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.Addresses.Add(address);
        context.SaveChanges();

        Assert.Equal(1, context.Addresses.Count());

        var existingAddress = context.Addresses.Include(a => a.Client).FirstOrDefault();
        Assert.NotNull(existingAddress);
        Assert.Equal("Ukraine", existingAddress!.Country);
        Assert.Equal("Vinnytsia", existingAddress.Region);
        Assert.Equal("Vinnytsia", existingAddress.City);
        Assert.Equal("Soborna", existingAddress.Street);
        Assert.Equal("10", existingAddress.Building);
        Assert.NotNull(existingAddress.Client);
    }

    [Fact]
    public void CreateAddress_WithNullableFields()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "test@test.com",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };

        var address = new Address
        {
            Country = "Ukraine",
            Region = "Kyiv",
            City = "Kyiv",
            Street = "Khreshchatyk",
            Building = "1",
            Client = client,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.Addresses.Add(address);
        context.SaveChanges();

        var existingAddress = context.Addresses.FirstOrDefault();
        Assert.NotNull(existingAddress);
        Assert.Null(existingAddress!.Area);
        Assert.Null(existingAddress.Apartment);
        Assert.Null(existingAddress.Entrance);
        Assert.Null(existingAddress.Room);
    }

    [Fact]
    public void UpdateAddress()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "test@test.com",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };

        var address = new Address
        {
            Country = "Ukraine",
            Region = "Vinnytsia",
            City = "Vinnytsia",
            Street = "Soborna",
            Building = "10",
            Client = client
        };

        context.Addresses.Add(address);
        context.SaveChanges();

        address.Street = "Hrushevskoho";
        address.Building = "20";
        context.Addresses.Update(address);
        context.SaveChanges();

        var updatedAddress = context.Addresses.First();
        Assert.Equal("Hrushevskoho", updatedAddress.Street);
        Assert.Equal("20", updatedAddress.Building);
    }

    [Fact]
    public void DeleteAddress()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "test@test.com",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };

        var address = new Address
        {
            Country = "Ukraine",
            Region = "Vinnytsia",
            City = "Vinnytsia",
            Street = "Soborna",
            Building = "10",
            Client = client
        };

        context.Addresses.Add(address);
        context.SaveChanges();

        Assert.Equal(1, context.Addresses.Count());

        context.Addresses.Remove(address);
        context.SaveChanges();

        Assert.Equal(0, context.Addresses.Count());
    }

    [Fact]
    public void Client_OneToOne_Address()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "test@test.com",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };

        var address = new Address
        {
            Country = "Ukraine",
            Region = "Vinnytsia",
            City = "Vinnytsia",
            Street = "Soborna",
            Building = "10",
            Client = client
        };

        context.Addresses.Add(address);
        context.SaveChanges();

        var savedClient = context.Clients.Include(c => c.Address).First();
        Assert.NotNull(savedClient.Address);
        Assert.Equal("Ukraine", savedClient.Address!.Country);
        Assert.Equal(savedClient.Id, savedClient.Address.ClientId);
    }
}