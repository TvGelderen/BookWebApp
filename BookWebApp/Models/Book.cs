using System.ComponentModel.DataAnnotations;

namespace BookWebApp.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Author { get; set; }
        public int Year { get; set; }
        public string? Summary { get; set; }
    }
}
