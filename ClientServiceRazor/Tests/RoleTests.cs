using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Users.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClientServiceRazor.Tests;

public class RoleTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateRole()
    {
        using var context = GetDbContext();
        var role = new Role
        {
            Name = "Admin"
        };

        context.Roles.Add(role);
        context.SaveChanges();

        Assert.Equal(1, context.Roles.Count());

        var existingRole = context.Roles.FirstOrDefault();
        Assert.NotNull(existingRole);
        Assert.Equal("Admin", existingRole!.Name);
    }

    [Fact]
    public void GetRoles()
    {
        using var context = GetDbContext();
        var roles = new[]
        {
            new Role { Name = "Admin" },
            new Role { Name = "User" }
        };

        context.Roles.AddRange(roles);
        context.SaveChanges();

        var allRoles = context.Roles.ToList();
        Assert.Equal(2, allRoles.Count);
        Assert.Equal("Admin", allRoles[0].Name);
        Assert.Equal("User", allRoles[1].Name);
    }

    [Fact]
    public void UpdateRole()
    {
        using var context = GetDbContext();
        var role = new Role
        {
            Name = "Moderator"
        };

        context.Roles.Add(role);
        context.SaveChanges();

        role.Name = "SuperModerator";
        context.Roles.Update(role);
        context.SaveChanges();

        var updatedRole = context.Roles.First();
        Assert.Equal("SuperModerator", updatedRole.Name);
    }

    [Fact]
    public void DeleteRole()
    {
        using var context = GetDbContext();
        var role = new Role
        {
            Name = "Guest"
        };

        context.Roles.Add(role);
        context.SaveChanges();

        Assert.Equal(1, context.Roles.Count());

        context.Roles.Remove(role);
        context.SaveChanges();

        Assert.Equal(0, context.Roles.Count());
    }
}