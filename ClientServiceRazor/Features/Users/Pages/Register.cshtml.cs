using ClientServiceRazor.Features.Users.Services;
using ClientServiceRazor.Features.Users.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace ClientServiceRazor.Features.Users.Pages;
public class Register : PageModel
{
    private readonly UserService _userService;
    public Register(UserService userService)
    {
        _userService = userService;
    }
    [BindProperty]
    public RegisterViewModel Input { get; set; } = new();
    public string Message { get; set; } = string.Empty;
    public void OnGet()
    {
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Повертаємося на сторінку з помилками валідації
            return Page();
        }
        try
        {
            // Створюємо користувача
            await _userService.RegisterAsync(Input.Login, Input.Password, email: Input.Email);
            Message = "Користувача успішно зареєстровано!";
            // Перенаправляємо на сторінку логіну
            return RedirectToPage("/Features/Users/Pages/Login");
        }
        catch (Exception ex)
        {
            Message = $"Помилка: {ex.Message}";
            return Page();
        }
    }
}