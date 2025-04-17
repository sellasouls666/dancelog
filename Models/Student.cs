using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace dancelog.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(255, ErrorMessage = "Длина не должна превышать 255 символов")]
        [Display(Name = "Фамилия")]
        public required string Surname { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(255, ErrorMessage = "Длина не должна превышать 255 символов")]
        [Display(Name = "Имя")]
        public required string Name { get; set; }

        [Display(Name = "Отчество")]
        public string? Patronymic { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Группа")]
        public int GroupId { get; set; }  // Изменили с Group на GroupId

        public Group? Group { get; set; }  // Навигационное свойство

        [EmailAddress(ErrorMessage = "Некорректный email адрес")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Номер должен содержать 10-11 цифр")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Номер должен содержать только цифры")]
        [Display(Name = "Номер телефона")]
        public required string PhoneNumber { get; set; }
    }
}
