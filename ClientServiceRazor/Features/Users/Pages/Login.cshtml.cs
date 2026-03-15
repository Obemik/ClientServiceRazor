using ClientServiceRazor.Features.Users.Services;
using ClientServiceRazor.Features.Users.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace ClientServiceRazor.Features.Users.Pages;
public class Login : PageModel
{
    private readonly UserService _userService;
    public Login(UserService userService)
    {
        _userService = userService;
    }
    [BindProperty]
    public LoginViewModel Input { get; set; } = new();
    public string Message { get; set; } = string.Empty;
    public void OnGet()
    {
        // Очистка сесії на GET
        HttpContext.Session.Clear();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var user = await _userService.AuthenticateAsync(Input.Login, Input.Password);
        if (user == null)
        {
            Message = "Неправильний логін або пароль.";
            return Page();
        }
        // Зберігаємо UserId в сесії
        HttpContext.Session.Set("UserId", BitConverter.GetBytes(user.Id));
        // Можна зберегти роль та статус у сесії або в Items через Middleware
        // Після успішного логіну на головну
        return RedirectToPage("/Features/Clients/Pages/Index");
    }
}