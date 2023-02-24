using System.ComponentModel.DataAnnotations;

namespace Assignment_3.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string IMBDLink { get; set; }
        public string Genre { get; set; }
        public DateTime dateOnly { get; set; }
        public byte[]? Poster { get; set; }

    }
}
