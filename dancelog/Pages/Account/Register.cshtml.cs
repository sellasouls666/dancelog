using dancelog.Data;
using dancelog.Models.Auth;
using dancelog.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace dancelog.Pages.Account
{
    public class RegisterPageModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RegisterPageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RegisterModel Input { get; set; } = new();

        public async Task OnGet()
        {
            Input = new RegisterModel
            {
                AvailableGroups = await GetGroupsSelectList(),
                SelectedRole = "Ученик"
            };
        }

        private async Task<SelectList> GetGroupsSelectList()
        {
            var groups = await _context.Groups
                .OrderBy(g => g.Name)
                .ToListAsync();
            return new SelectList(groups, "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Input.AvailableGroups = await GetGroupsSelectList();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Проверка существования пользователя
            if (await _context.AuthUsers.AnyAsync(u => u.Email == Input.Email))
            {
                ModelState.AddModelError("Input.Email", "Пользователь с таким email уже существует");
                await GetGroupsSelectList();
                return Page();
            }

            // Определяем роль
            string role = await _context.AuthUsers.AnyAsync() ? Input.SelectedRole : "Админ";

            // Создаем пользователя
            var user = new AuthUser
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                MiddleName = Input.MiddleName,
                Email = Input.Email,
                Password = Input.Password, // В реальном приложении хэшируйте пароль!
                Role = role,
                GroupId = Input.SelectedRole == "Ученик" ? Input.GroupId : null
            };

            // Если регистрируется студент - создаем запись в Students
            if (Input.SelectedRole == "Ученик")
            {
                var student = new Student
                {
                    Surname = Input.LastName,
                    Name = Input.FirstName,
                    Patronymic = Input.MiddleName,
                    GroupId = Input.GroupId.Value,
                    Email = Input.Email,
                    AuthUser = user
                };

                _context.Students.Add(student);
                user.Student = student;
            }

            _context.AuthUsers.Add(user);
            await _context.SaveChangesAsync();

            await Authenticate(user);
            return RedirectToPage("/Index");
        }

        private async Task Authenticate(AuthUser user)
        {
            var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.FirstName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

            if (user.GroupId.HasValue)
            {
                claims.Add(new("GroupId", user.GroupId.Value.ToString()));
            }

            var identity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);
        }
    }
}