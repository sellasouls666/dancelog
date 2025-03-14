using System.ComponentModel.DataAnnotations;
namespace dancelog.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public required string Name { get; set; }
    
    }
}
