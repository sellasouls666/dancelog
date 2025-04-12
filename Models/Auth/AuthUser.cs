using System.ComponentModel.DataAnnotations;

namespace dancelog.Models.Auth
{
    public class AuthUser
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указан Email")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Роль обязательна")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Не указана фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Не указано имя")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        // Вычисляемое свойство для полного имени
        public string FullName => $"{LastName} {FirstName}{(string.IsNullOrWhiteSpace(MiddleName) ? "" : " " + MiddleName)}";
    }
}
