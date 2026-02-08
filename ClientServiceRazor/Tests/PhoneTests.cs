using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClientServiceRazor.Tests;

public class PhoneTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreatePhone()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "test@test.com",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };
        
        var phone = new Phone
        {
            Number = "0991234567",
            CountryCode = CountryCode.UA,
            Client = client,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        context.Phones.Add(phone);
        context.SaveChanges();

        Assert.Equal(1, context.Phones.Count());
        
        var existingPhone = context.Phones.Include(p => p.Client).FirstOrDefault();
        Assert.NotNull(existingPhone);
        Assert.Equal("0991234567", existingPhone!.Number);
        Assert.Equal(CountryCode.UA, existingPhone.CountryCode);
        Assert.NotNull(existingPhone.Client);
        Assert.Equal("Surname", existingPhone.Client.Surname);
    }

    [Fact]
    public void GetPhones()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "test@test.com",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };

        var phones = new[]
        {
            new Phone { Number = "111", CountryCode = CountryCode.UA, Client = client },
            new Phone { Number = "222", CountryCode = CountryCode.US, Client = client }
        };
        
        context.Phones.AddRange(phones);
        context.SaveChanges();

        var allPhones = context.Phones.ToList();
        Assert.Equal(2, allPhones.Count);
        Assert.Equal("111", allPhones[0].Number);
        Assert.Equal("222", allPhones[1].Number);
    }

    [Fact]
    public void UpdatePhone()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "test@test.com",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };
        
        var phone = new Phone
        {
            Number = "111",
            CountryCode = CountryCode.UA,
            Client = client
        };
        
        context.Phones.Add(phone);
        context.SaveChanges();

        phone.Number = "222";
        phone.CountryCode = CountryCode.US;
        context.Phones.Update(phone);
        context.SaveChanges();

        var updatedPhone = context.Phones.First();
        Assert.Equal("222", updatedPhone.Number);
        Assert.Equal(CountryCode.US, updatedPhone.CountryCode);
    }

    [Fact]
    public void DeletePhone()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "test@test.com",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };
        
        var phone = new Phone
        {
            Number = "111",
            CountryCode = CountryCode.UA,
            Client = client
        };
        
        context.Phones.Add(phone);
        context.SaveChanges();

        Assert.Equal(1, context.Phones.Count());

        context.Phones.Remove(phone);
        context.SaveChanges();

        Assert.Equal(0, context.Phones.Count());
    }
}