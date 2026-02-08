using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClientServiceRazor.Tests;

public class FinanceAccountTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateFinanceAccount()
    {
        using var context = GetDbContext();
        var account = new FinanceAccount
        {
            Balance = 1000.50m,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.FinanceAccounts.Add(account);
        context.SaveChanges();

        Assert.Equal(1, context.FinanceAccounts.Count());

        var existingAccount = context.FinanceAccounts.FirstOrDefault();
        Assert.NotNull(existingAccount);
        Assert.Equal(1000.50m, existingAccount!.Balance);
    }

    [Fact]
    public void GetFinanceAccounts()
    {
        using var context = GetDbContext();
        var accounts = new[]
        {
            new FinanceAccount { Balance = 100m },
            new FinanceAccount { Balance = 200m }
        };

        context.FinanceAccounts.AddRange(accounts);
        context.SaveChanges();

        var allAccounts = context.FinanceAccounts.ToList();
        Assert.Equal(2, allAccounts.Count);
        Assert.Equal(100m, allAccounts[0].Balance);
        Assert.Equal(200m, allAccounts[1].Balance);
    }

    [Fact]
    public void UpdateFinanceAccount()
    {
        using var context = GetDbContext();
        var account = new FinanceAccount
        {
            Balance = 100m
        };

        context.FinanceAccounts.Add(account);
        context.SaveChanges();

        account.Balance = 500m;
        context.FinanceAccounts.Update(account);
        context.SaveChanges();

        var updatedAccount = context.FinanceAccounts.First();
        Assert.Equal(500m, updatedAccount.Balance);
    }

    [Fact]
    public void DeleteFinanceAccount()
    {
        using var context = GetDbContext();
        var account = new FinanceAccount
        {
            Balance = 100m
        };

        context.FinanceAccounts.Add(account);
        context.SaveChanges();

        Assert.Equal(1, context.FinanceAccounts.Count());

        context.FinanceAccounts.Remove(account);
        context.SaveChanges();

        Assert.Equal(0, context.FinanceAccounts.Count());
    }

    [Fact]
    public void Client_ManyToMany_FinanceAccount()
    {
        using var context = GetDbContext();
        var client = new Client
        {
            Surname = "Surname",
            FirstName = "FirstName",
            Email = "test@test.com",
            BirthDate = DateOnly.FromDateTime(DateTime.Now)
        };

        var account1 = new FinanceAccount { Balance = 100m };
        var account2 = new FinanceAccount { Balance = 200m };

        var link1 = new ClientFinanceAccount
        {
            Client = client,
            FinanceAccount = account1
        };

        var link2 = new ClientFinanceAccount
        {
            Client = client,
            FinanceAccount = account2
        };

        context.ClientFinanceAccounts.AddRange(link1, link2);
        context.SaveChanges();

        var savedClient = context.Clients
            .Include(c => c.ClientFinanceAccounts)
            .ThenInclude(cfa => cfa.FinanceAccount)
            .First();

        Assert.Equal(2, savedClient.ClientFinanceAccounts.Count);
        Assert.Contains(savedClient.ClientFinanceAccounts, 
            cfa => cfa.FinanceAccount.Balance == 100m);
        Assert.Contains(savedClient.ClientFinanceAccounts, 
            cfa => cfa.FinanceAccount.Balance == 200m);
    }
}