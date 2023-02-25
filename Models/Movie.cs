using System.ComponentModel.DataAnnotations;
using Assignment_3.Interface;

namespace Assignment_3.Models
{
    public class Movie : ITweetable
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string IMBDLink { get; set; }
        public string Genre { get; set; }
        public DateTime dateOnly { get; set; }
        public byte[]? Poster { get; set; }

        public string SearchTerm()
        {
            return Title;
        }
    }
}
