using System.ComponentModel.DataAnnotations;

namespace dancelog.Models.Auth
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указана фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Не указано имя")]
        public string FirstName { get; set; }

        // Отчество можно оставить необязательным
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Не указан Email")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Необходимо подтвердить пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        // Добавляем выбор роли
        // Предусматриваем, что пользователь может выбрать либо "Ученик", либо "Учитель"
        [Required(ErrorMessage = "Выберите роль")]
        [Display(Name = "Роль")]
        public string SelectedRole { get; set; }
    }
}
