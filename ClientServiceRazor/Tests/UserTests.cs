using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Users.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClientServiceRazor.Tests;

public class UserTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateUser()
    {
        using var context = GetDbContext();
        var status = new Status { Name = "Active" };
        var role = new Role { Name = "Admin" };

        var user = new User
        {
            Login = "testuser",
            Password = "hashedpassword",
            Email = "test@test.com",
            Status = status,
            Role = role,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.Users.Add(user);
        context.SaveChanges();

        Assert.Equal(1, context.Users.Count());

        var existingUser = context.Users
            .Include(u => u.Status)
            .Include(u => u.Role)
            .FirstOrDefault();

        Assert.NotNull(existingUser);
        Assert.Equal("testuser", existingUser!.Login);
        Assert.Equal("test@test.com", existingUser.Email);
        Assert.Equal("Active", existingUser.Status.Name);
        Assert.Equal("Admin", existingUser.Role.Name);
    }

    [Fact]
    public void GetUsers()
    {
        using var context = GetDbContext();
        var status = new Status { Name = "Active" };
        var role = new Role { Name = "User" };

        var users = new[]
        {
            new User 
            { 
                Login = "user1", 
                Password = "pass1", 
                Email = "user1@test.com",
                Status = status,
                Role = role
            },
            new User 
            { 
                Login = "user2", 
                Password = "pass2", 
                Email = "user2@test.com",
                Status = status,
                Role = role
            }
        };

        context.Users.AddRange(users);
        context.SaveChanges();

        var allUsers = context.Users.ToList();
        Assert.Equal(2, allUsers.Count);
        Assert.Equal("user1", allUsers[0].Login);
        Assert.Equal("user2", allUsers[1].Login);
    }

    [Fact]
    public void UpdateUser()
    {
        using var context = GetDbContext();
        var status = new Status { Name = "Active" };
        var role = new Role { Name = "User" };

        var user = new User
        {
            Login = "oldlogin",
            Password = "oldpass",
            Email = "old@test.com",
            Status = status,
            Role = role
        };

        context.Users.Add(user);
        context.SaveChanges();

        user.Login = "newlogin";
        user.Email = "new@test.com";
        context.Users.Update(user);
        context.SaveChanges();

        var updatedUser = context.Users.First();
        Assert.Equal("newlogin", updatedUser.Login);
        Assert.Equal("new@test.com", updatedUser.Email);
    }

    [Fact]
    public void DeleteUser()
    {
        using var context = GetDbContext();
        var status = new Status { Name = "Active" };
        var role = new Role { Name = "User" };

        var user = new User
        {
            Login = "testuser",
            Password = "pass",
            Email = "test@test.com",
            Status = status,
            Role = role
        };

        context.Users.Add(user);
        context.SaveChanges();

        Assert.Equal(1, context.Users.Count());

        context.Users.Remove(user);
        context.SaveChanges();

        Assert.Equal(0, context.Users.Count());
    }

    [Fact]
    public void User_ManyToOne_Status()
    {
        using var context = GetDbContext();
        var status = new Status { Name = "Active" };
        var role = new Role { Name = "User" };

        var user1 = new User
        {
            Login = "user1",
            Password = "pass1",
            Email = "user1@test.com",
            Status = status,
            Role = role
        };

        var user2 = new User
        {
            Login = "user2",
            Password = "pass2",
            Email = "user2@test.com",
            Status = status,
            Role = role
        };

        context.Users.AddRange(user1, user2);
        context.SaveChanges();

        var savedStatus = context.Statuses
            .Include(s => s.Users)
            .First();

        Assert.Equal(2, savedStatus.Users.Count);
        Assert.Contains(savedStatus.Users, u => u.Login == "user1");
        Assert.Contains(savedStatus.Users, u => u.Login == "user2");
    }

    [Fact]
    public void User_ManyToOne_Role()
    {
        using var context = GetDbContext();
        var status = new Status { Name = "Active" };
        var role = new Role { Name = "Admin" };

        var user1 = new User
        {
            Login = "admin1",
            Password = "pass1",
            Email = "admin1@test.com",
            Status = status,
            Role = role
        };

        var user2 = new User
        {
            Login = "admin2",
            Password = "pass2",
            Email = "admin2@test.com",
            Status = status,
            Role = role
        };

        context.Users.AddRange(user1, user2);
        context.SaveChanges();

        var savedRole = context.Roles
            .Include(r => r.Users)
            .First();

        Assert.Equal(2, savedRole.Users.Count);
        Assert.Contains(savedRole.Users, u => u.Login == "admin1");
        Assert.Contains(savedRole.Users, u => u.Login == "admin2");
    }
}