using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dancelog.Data;
using dancelog.Models;
using dancelog.Models.Auth;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

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
        public AuthUser UserModel { get; set; }

        [BindProperty]
        public Student? StudentModel { get; set; }

        public string CurrentRole { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Account/Login");
            }

            // «агружаем пользовател€ с св€занными данными студента
            var currentUser = await _context.AuthUsers
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser == null)
            {
                return NotFound();
            }

            UserModel = currentUser;
            CurrentRole = currentUser.Role;

            // ≈сли пользователь - студент, загружаем его данные
            if (currentUser.Role == "”ченик" && currentUser.Student != null)
            {
                StudentModel = currentUser.Student;
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                // ¬осстанавливаем роль при повторном отображении страницы
                var postUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var postUser = await _context.AuthUsers
                    .Include(u => u.Student)
                    .FirstOrDefaultAsync(u => u.Email == postUserEmail);

                if (postUser != null)
                {
                    CurrentRole = postUser.Role;
                }

                return Page();
            }

            var updateEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var userToUpdate = await _context.AuthUsers
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.Email == updateEmail);

            if (userToUpdate == null)
            {
                return NotFound();
            }

            // ќбновл€ем основные данные пользовател€
            userToUpdate.FirstName = UserModel.FirstName;
            userToUpdate.LastName = UserModel.LastName;
            userToUpdate.MiddleName = UserModel.MiddleName;

            // ≈сли пользователь - студент, обновл€ем и его данные
            if (userToUpdate.Role == "”ченик" && userToUpdate.Student != null)
            {
                userToUpdate.Student.Surname = UserModel.LastName;
                userToUpdate.Student.Name = UserModel.FirstName;
                userToUpdate.Student.Patronymic = UserModel.MiddleName;
                userToUpdate.Student.Email = UserModel.Email;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("Profile");
        }
    }
}