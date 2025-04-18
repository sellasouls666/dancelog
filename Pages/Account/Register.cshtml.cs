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
            await LoadGroups();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadGroups();

            // Дополнительная валидация для студентов
            if (Input.SelectedRole == "Ученик" && !Input.GroupId.HasValue)
            {
                ModelState.AddModelError("Input.GroupId", "Для студентов необходимо выбрать группу");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Проверка существования пользователя
            if (await _context.AuthUsers.AnyAsync(u => u.Email == Input.Email))
            {
                ModelState.AddModelError("Input.Email", "Пользователь с таким email уже существует");
                return Page();
            }

            // Проверка существования группы (если выбрана)
            if (Input.GroupId.HasValue &&
                !await _context.Groups.AnyAsync(g => g.Id == Input.GroupId.Value))
            {
                ModelState.AddModelError("Input.GroupId", "Выбранная группа не существует");
                return Page();
            }

            // Создаем пользователя
            var user = new AuthUser
            {
                LastName = Input.LastName,
                FirstName = Input.FirstName,
                MiddleName = Input.MiddleName,
                Email = Input.Email,
                Password = Input.Password, // В реальном приложении нужно хэшировать!
                Role = _context.AuthUsers.Any() ? Input.SelectedRole : "Админ",
                GroupId = Input.SelectedRole == "Ученик" ? Input.GroupId : null
            };

            _context.AuthUsers.Add(user);
            await _context.SaveChangesAsync();

            await Authenticate(user);
            return RedirectToPage("/Index");
        }

        private async Task LoadGroups()
        {
            Input.AvailableGroups = new SelectList(
                await _context.Groups.OrderBy(g => g.Name).ToListAsync(),
                nameof(Group.Id),
                nameof(Group.Name));
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