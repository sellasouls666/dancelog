using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dancelog.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Занятие")]
        public int LessonId { get; set; }        // явное поле FK
        [ForeignKey(nameof(LessonId))]
        public Lesson Lesson { get; set; } = null!;

        [Required]
        [Display(Name = "Студент")]
        public int StudentId { get; set; }       // явное поле FK
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; } = null!;

        [Required]
        [StringLength(255)]
        [Display(Name = "Статус")]
        public string Status { get; set; } = string.Empty;   // строковое поле (nvarchar(255))
    }
}
