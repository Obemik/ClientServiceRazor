using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Users.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace ClientServiceRazor.Features.Users.Services;
public class UserService
{
    private readonly AppDbContext _dbContext;
    public UserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<User?> AuthenticateAsync(string login, string password)
    {
        var hash = PasswordHelper.__helperComputeHash(password);
        return await _dbContext.Users
            .Include(u => u.Role)
            .Include(u => u.Status)
            .FirstOrDefaultAsync(u => u.Login == login && u.Password == hash);
    }
    public async Task<User?> RegisterAsync(string login, string password, string email)
    {
        var user = new User
        {
            Login = login,
            Password = PasswordHelper.__helperComputeHash(password),
            RoleId = 1,
            StatusId = 1,
            Email = email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }
    public async Task<User?> GetUserByIdAsync(uint userId)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .Include(u => u.Status)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }
}
public static class PasswordHelper
{
    public static string __helperComputeHash(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = sha256.ComputeHash(bytes);
        var builder = new StringBuilder();
        foreach (var b in hashBytes)
        {
            builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
    }
}