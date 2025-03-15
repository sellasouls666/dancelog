using System.ComponentModel.DataAnnotations;

namespace dancelog.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите название курса.")]
        [StringLength(255, ErrorMessage = "Название группы должно быть не длиннее 255 символов.")]
        [Display(Name = "Название курса")]
        public required string Name { get; set; }

        [Required]
        [Display(Name = "Курс")]
        public required Course Course { get; set; }

    }
}
