using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register MovieDBContext with SQL Server
builder.Services.AddDbContext<MovieDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieDBContext")));

var app = builder.Build();

// Инициализация базы данных тестовыми данными
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<MovieDBContext>();
        
        // Применяем миграции или создаем базу данных
        context.Database.EnsureCreated();
        
        // Заполняем тестовыми данными
        MovieInitializer.Initialize(services);
        
        Console.WriteLine("База данных успешно инициализирована");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при инициализации базы данных: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Выводим URL для доступа к приложению
var urls = app.Urls;
Console.WriteLine("=== MVC Movie App Started ===");
Console.WriteLine($"Application started. Access at: http://localhost:5000");
if (Environment.GetEnvironmentVariable("CODESPACE_NAME") != null)
{
    var codespaceName = Environment.GetEnvironmentVariable("CODESPACE_NAME");
    Console.WriteLine($"Codespaces URL: https://{codespaceName}-5000.app.github.dev");
}
Console.WriteLine("=============================");

app.Run();