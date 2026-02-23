using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register MovieDBContext with SQL Server
builder.Services.AddDbContext<MovieDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieDBContext")));

var app = builder.Build();

// Автоматическое создание/обновление базы данных при запуске
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MovieDBContext>();
    try
    {
        // Создаем базу данных, если её нет
        dbContext.Database.EnsureCreated();
        // Или используйте Migrate() если у вас есть миграции
        // dbContext.Database.Migrate();
        Console.WriteLine("База данных успешно создана/проверена");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при работе с базой данных: {ex.Message}");
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

app.Run();