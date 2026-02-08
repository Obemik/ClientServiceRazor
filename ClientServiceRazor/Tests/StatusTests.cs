using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Users.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClientServiceRazor.Tests;

public class StatusTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateStatus()
    {
        using var context = GetDbContext();
        var status = new Status
        {
            Name = "Active"
        };

        context.Statuses.Add(status);
        context.SaveChanges();

        Assert.Equal(1, context.Statuses.Count());

        var existingStatus = context.Statuses.FirstOrDefault();
        Assert.NotNull(existingStatus);
        Assert.Equal("Active", existingStatus!.Name);
    }

    [Fact]
    public void GetStatuses()
    {
        using var context = GetDbContext();
        var statuses = new[]
        {
            new Status { Name = "Active" },
            new Status { Name = "Inactive" }
        };

        context.Statuses.AddRange(statuses);
        context.SaveChanges();

        var allStatuses = context.Statuses.ToList();
        Assert.Equal(2, allStatuses.Count);
        Assert.Equal("Active", allStatuses[0].Name);
        Assert.Equal("Inactive", allStatuses[1].Name);
    }

    [Fact]
    public void UpdateStatus()
    {
        using var context = GetDbContext();
        var status = new Status
        {
            Name = "Pending"
        };

        context.Statuses.Add(status);
        context.SaveChanges();

        status.Name = "Approved";
        context.Statuses.Update(status);
        context.SaveChanges();

        var updatedStatus = context.Statuses.First();
        Assert.Equal("Approved", updatedStatus.Name);
    }

    [Fact]
    public void DeleteStatus()
    {
        using var context = GetDbContext();
        var status = new Status
        {
            Name = "Temporary"
        };

        context.Statuses.Add(status);
        context.SaveChanges();

        Assert.Equal(1, context.Statuses.Count());

        context.Statuses.Remove(status);
        context.SaveChanges();

        Assert.Equal(0, context.Statuses.Count());
    }
}