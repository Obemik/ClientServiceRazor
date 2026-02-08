using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClientServiceRazor.Tests;

public class ClientTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateClient()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "Email",
            Patronymic = "Patronymic",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };
        context.Clients.Add(client);
        context.SaveChanges();
        // Assert.Equal(1, client.Id);
        Assert.Equal(1, context.Clients.Count());
        // Отримування записаного клієнта 
        var existingClient = context.Clients.FirstOrDefault();
        Assert.NotNull(existingClient);
        Assert.Equal("Surname", existingClient!.Surname);
        Assert.Equal("FirstName", existingClient.FirstName);
        Assert.Equal("Email", existingClient.Email);
        Assert.Equal("Patronymic", existingClient.Patronymic);
        Assert.Equal(DateOnly.FromDateTime(DateTime.Now), existingClient.BirthDate);
    }

    [Fact]
    public void UpdateClient()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "Email",
            Patronymic = "Patronymic",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };
        context.Clients.Add(client);
        context.SaveChanges();
        // Змінити дані клієнта
        client.Surname = "Surname2";
        client.FirstName = "FirstName2";
        //...
        context.Clients.Update(client);
        context.SaveChanges();
        //
        var updatedClient = context.Clients.First(c => c.Email == "Email");
        Assert.Equal("Surname2", updatedClient.Surname);
        Assert.Equal("FirstName2", updatedClient.FirstName);
    }
    
    [Fact]
    public void DeleteClient()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "Email",
            Patronymic = "Patronymic",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };
        context.Clients.Add(client);
        context.SaveChanges();
    
        Assert.Equal(1, context.Clients.Count());
    
        context.Clients.Remove(client);
        context.SaveChanges();
    
        Assert.Equal(0, context.Clients.Count());
        var deletedClient = context.Clients.FirstOrDefault(c => c.Email == "Email");
        Assert.Null(deletedClient);
    }

    [Fact]
    public void GetClients()
    {
        using var context = GetDbContext();
        var clients = new[]
        {
            new Client
            {
                Surname = "Surname1",
                FirstName = "FirstName1",
                Email = "Email1",
                Patronymic = "Patronymic1",
                BirthDate = DateOnly.FromDateTime(DateTime.Now)
            },
            new Client
            {
                Surname = "Surname2",
                FirstName = "FirstName2",
                Email = "Email2",
                Patronymic = "Patronymic2",
                BirthDate = DateOnly.FromDateTime(DateTime.Now)
            }
         };
        context.Clients.AddRange(clients);
        context.SaveChanges();
        var allClients = context.Clients.ToList();
        Assert.Equal(2, allClients.Count);
        clients[0].Surname = "S";
        Assert.Equal(clients[0], allClients[0]);
        Assert.Equal(clients[1], allClients[1]);
    }

    [Fact]
    public void CreateClient_WithPhones()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "Email",
            Patronymic = "Patronymic",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };
        var phone1 = new Phone
        {
            Number = "111",
            Client = client
        };
        var phone2 = new Phone
        {
            Number = "222",
            Client = client
        };
        context.Phones.AddRange(phone1, phone2 );
        context.SaveChanges();
        Assert.Equal(2, context.Phones.Count());
        
        var savedClient = context.Clients.Include(c => c.Phones).First();
        Assert.NotNull(savedClient.Phones);
        Assert.NotEmpty(savedClient.Phones);
        Assert.Equal("111", savedClient.Phones.First().Number);
    }
}