using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dancelog.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать группу")]
        [Display(Name = "Группа")]
        public int GroupId { get; set; } // Внешний ключ

        [ForeignKey("GroupId")]
        [Display(Name = "Группа")]
        public Group? Group { get; set; }

        [Required(ErrorMessage = "Дата и время обязательны")]
        [Display(Name = "Дата и время")]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}