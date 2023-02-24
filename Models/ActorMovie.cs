
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_3.Models
{
    public class ActorMovie
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Actor")]
        public int ActorId { get; set; }
        [Required]
        public Actor Actor { get; set; }
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        [Required]
        public Movie Movie { get; set; }
    }
}
