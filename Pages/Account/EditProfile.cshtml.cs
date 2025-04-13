using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dancelog.Data;
using dancelog.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;

namespace dancelog.Pages.Account
{
    [Authorize]
    public class EditProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditProfileModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string CurrentRole { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Имя обязательно")]
            [Display(Name = "Имя")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Фамилия обязательна")]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }

            [Display(Name = "Отчество")]
            [Required(ErrorMessage = "Отчество обязательно")]
            public string MiddleName { get; set; }

            [Required(ErrorMessage = "Email обязателен")]
            [EmailAddress(ErrorMessage = "Некорректный email")]
            [Display(Name = "Email")]
            public string Email { get; set; }
        }

        public IActionResult OnGet()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Account/Login");
            }

            var currentUser = _context.AuthUsers.FirstOrDefault(u => u.Email == userEmail);
            if (currentUser == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                MiddleName = currentUser.MiddleName,
                Email = currentUser.Email
            };

            CurrentRole = currentUser.Role;

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Восстанавливаем роль при повторном отображении страницы
                var postUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var postUser = _context.AuthUsers.FirstOrDefault(u => u.Email == postUserEmail);
                if (postUser != null)
                {
                    CurrentRole = postUser.Role;
                }

                return Page();
            }

            var updateEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var userToUpdate = _context.AuthUsers.FirstOrDefault(u => u.Email == updateEmail);
            if (userToUpdate == null)
            {
                return NotFound();
            }

            // Обновляем только изменяемые поля
            userToUpdate.FirstName = Input.FirstName;
            userToUpdate.LastName = Input.LastName;
            userToUpdate.MiddleName = Input.MiddleName;

            _context.SaveChanges();

            return RedirectToPage("Profile");
        }
    }
}