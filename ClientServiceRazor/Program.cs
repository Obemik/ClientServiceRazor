using Microsoft.EntityFrameworkCore;
using ClientServiceRazor.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddRazorPages();

builder.Services.AddRazorPages(options =>
{
    /*
    options.RootDirectory = "/";
    Вказує кореневу папку для пошуку сторінок Razor Pages.
    За замовчуванням сторінки шукаються в "/Pages".
    "/" встановлює кореневу папку проєкту як базову для пошуку Razor Pages.
    */
    options.RootDirectory = "/";
    /*
    Прибирання службової частини шаблонів маршрутів Razor Pages, наприклад:
    Старий шаблон -> Новий шаблон
    /Pages/ ->
    /Pages/Index -> Index
    /Pages/Privacy -> Privacy
    /Features/Clients/Pages/ -> Clients/
    /Features/Clients/Pages/Index -> Clients/Index
    /Features/Clients/Pages/Details -> Clients/Details
    */
    options.Conventions.AddFolderRouteModelConvention(
        "/",
        model =>
        {
            foreach (var selector in model.Selectors)
            {
                selector.AttributeRouteModel.Template =
                    selector.AttributeRouteModel.Template
                        .Replace("Features/", "")
                        .Replace("Pages/", "")
                        .Replace("Pages", "");
            }
        }
    );
    /*
    Явне налаштування маршрутів для Razor Pages.
    Вказування для кожної сторінки, який URL вона повинна мати
    Шаблон маршруту, скорочений вище -> URL в браузері
    */
    // options.Conventions.AddPageRoute("/Index", "/");
    // options.Conventions.AddPageRoute("/Privacy", "Privacy");
    // options.Conventions.AddPageRoute("/Clients/Index", "Clients");
    // options.Conventions.AddPageRoute("/Clients/Details", "Clients/{id}");
});


//Підключення DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.MapGet("/",
    context =>
    {
        context.Response.Redirect("/Clients");
        return Task.CompletedTask;
    }
);


app.Run();