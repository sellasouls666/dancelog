using System.ComponentModel.DataAnnotations;

namespace dancelog.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        [Required]
        public required Lesson Lesson { get; set; }

        [Required]
        [StringLength(255)]
        public required Student Student { get; set; }

        [Required]
        public required string Status { get; set; }
    }
}
