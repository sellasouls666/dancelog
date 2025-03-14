using System.ComponentModel.DataAnnotations;

namespace dancelog.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        [Required]
        public required Course Course { get; set; }

        [Required]
        public required DateTime DateTime { get; set; }
    }
}
