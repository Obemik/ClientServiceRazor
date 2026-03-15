using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace ClientServiceRazor.Features.Users.Pages;
public class Logout : PageModel
{
    public IActionResult OnGet()
    {
        // Якщо ви зберігаєте сесію або куки
        // очищаємо всю сесію
        HttpContext.Session.Clear();
        // Якщо у вас є кастомна кука авторизації
        if (Request.Cookies.ContainsKey("auth"))
        {
            Response.Cookies.Delete("auth");
        }
        return Page();
    }
}