using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MvcMovie.Models
{
    public class MovieInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MovieDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<MovieDBContext>>()))
            {
                // Если база данных уже создана
                if (context.Movies.Any())
                {
                    return; // База данных уже заполнена
                }

                var movies = new List<Movie>
                {
                    new Movie
                    {
                        Title = "When Harry Met Sally",
                        ReleaseDate = DateTime.Parse("1989-1-11"),
                        Genre = "Romantic Comedy",
                        Rating = "R",
                        Price = 7.99M
                    },
                    new Movie
                    {
                        Title = "Ghostbusters",
                        ReleaseDate = DateTime.Parse("1984-3-13"),
                        Genre = "Comedy",
                        Rating = "PG",
                        Price = 8.99M
                    },
                    new Movie
                    {
                        Title = "Ghostbusters 2",
                        ReleaseDate = DateTime.Parse("1986-2-23"),
                        Genre = "Comedy",
                        Rating = "PG",
                        Price = 9.99M
                    },
                    new Movie
                    {
                        Title = "Rio Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-15"),
                        Genre = "Western",
                        Rating = "PG",
                        Price = 3.99M
                    },
                    new Movie
                    {
                        Title = "The Matrix",
                        ReleaseDate = DateTime.Parse("1999-3-31"),
                        Genre = "Sci-Fi",
                        Rating = "R",
                        Price = 12.99M
                    },
                    new Movie
                    {
                        Title = "The Godfather",
                        ReleaseDate = DateTime.Parse("1972-3-24"),
                        Genre = "Drama",
                        Rating = "R",
                        Price = 14.99M
                    }
                };

                context.Movies.AddRange(movies);
                context.SaveChanges();
            }
        }
    }
}