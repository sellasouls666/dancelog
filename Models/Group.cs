using System.ComponentModel.DataAnnotations;

namespace dancelog.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public required string Name { get; set; }

        [Required]
        public required Course Course { get; set; }
    }
}
