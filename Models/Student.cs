using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace dancelog.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public required string Name { get; set; }

        [Required]
        [StringLength(255)]
        public required string Surname { get; set; }
        public string Patronymic { get; set; }

        [Required]
        public required Group Group { get; set; }
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        public required string PhoneNumber { get; set; }
    }
}
