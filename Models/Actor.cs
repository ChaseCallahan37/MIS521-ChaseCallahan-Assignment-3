using System.ComponentModel.DataAnnotations;

namespace Assignment_3.Models
{
    public class Actor
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string IMBDLink { get; set; }

        public byte[]? Image { get; set; }
    }
}
