using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register MovieDBContext with SQLite (для Render)
var connectionString = builder.Configuration.GetConnectionString("MovieDBContext");
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = "Data Source=movies.db";
}
builder.Services.AddDbContext<MovieDBContext>(options =>
    options.UseSqlite(connectionString));

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
        
        // Заполняем тестовыми данными, если БД пуста
        if (!context.Movies.Any())
        {
            var movies = new List<Movie>
            {
                new Movie { Title = "When Harry Met Sally", ReleaseDate = DateTime.Parse("1989-1-11"), Genre = "Romantic Comedy", Rating = "R", Price = 7.99M },
                new Movie { Title = "Ghostbusters", ReleaseDate = DateTime.Parse("1984-3-13"), Genre = "Comedy", Rating = "PG", Price = 8.99M },
                new Movie { Title = "Ghostbusters 2", ReleaseDate = DateTime.Parse("1986-2-23"), Genre = "Comedy", Rating = "PG", Price = 9.99M },
                new Movie { Title = "Rio Bravo", ReleaseDate = DateTime.Parse("1959-4-15"), Genre = "Western", Rating = "PG", Price = 3.99M },
                new Movie { Title = "The Matrix", ReleaseDate = DateTime.Parse("1999-3-31"), Genre = "Sci-Fi", Rating = "R", Price = 12.99M },
                new Movie { Title = "The Godfather", ReleaseDate = DateTime.Parse("1972-3-24"), Genre = "Drama", Rating = "R", Price = 14.99M }
            };
            
            context.Movies.AddRange(movies);
            context.SaveChanges();
            Console.WriteLine("База данных успешно инициализирована тестовыми данными");
        }
        
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
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
Console.WriteLine($"=== MVC Movie App Started ===");
Console.WriteLine($"Application started. Access at: http://0.0.0.0:{port}");
Console.WriteLine($"Render URL: {Environment.GetEnvironmentVariable("RENDER_EXTERNAL_URL")}");
Console.WriteLine("=============================");

app.Run();