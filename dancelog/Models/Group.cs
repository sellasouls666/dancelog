using System.ComponentModel.DataAnnotations;

namespace dancelog.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите название группы.")]
        [StringLength(255, ErrorMessage = "Название группы должно быть не длиннее 255 символов.")]
        [Display(Name = "Название группы")]
        public required string Name { get; set; }

        [Required]
        [Display(Name = "Курс")]
        public int CourseId { get; set; } 
        public Course? Course { get; set; }
    }
}
