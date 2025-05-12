using System.ComponentModel.DataAnnotations;

namespace dancelogMVC.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите название курса.")]
        [StringLength(255, ErrorMessage = "Название курса не должно превышать 255 символов.")]
        [Display(Name = "Название курса")]
        public required string Name { get; set; }
    }
}
