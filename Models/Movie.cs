using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MvcMovie.Models
{
    public class Movie
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Release date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Genre must be specified")]
        [StringLength(30, ErrorMessage = "Genre cannot be longer than 30 characters")]
        [Display(Name = "Genre")]
        public string Genre { get; set; } = string.Empty;

        [Range(1, 100, ErrorMessage = "Price must be between $1 and $100")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = false)]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [StringLength(5, ErrorMessage = "Rating cannot be longer than 5 characters")]
        [Display(Name = "Rating")]
        public string Rating { get; set; } = string.Empty;
    }

    public class MovieDBContext : DbContext
    {
        public MovieDBContext(DbContextOptions<MovieDBContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
    }
}