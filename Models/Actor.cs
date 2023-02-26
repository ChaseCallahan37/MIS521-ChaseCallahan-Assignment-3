using System.ComponentModel.DataAnnotations;
using Assignment_3.Interface;

namespace Assignment_3.Models
{
    public class Actor : ITweetable
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string IMBDLink { get; set; }

        public byte[]? Image { get; set; }

        public string SearchTerm()
        {
            return Name;
        }
    }
}
