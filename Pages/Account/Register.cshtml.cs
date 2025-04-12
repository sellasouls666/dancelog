using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dancelog.Models.Auth;
using dancelog.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace dancelog.Pages.Account
{
    public class RegisterPageModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RegisterPageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Свойство для привязки данных из формы регистрации
        [BindProperty]
        public RegisterModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Проверяем, существует ли уже пользователь с таким Email
            var existingUser = _context.AuthUsers.FirstOrDefault(u => u.Email == Input.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Пользователь с таким Email уже существует");
                return Page();
            }

            // Определяем роль:
            // Если база данных не содержит ни одного пользователя, то роль = "Админ".
            // В противном случае используем выбранную пользователем роль ("Ученик" или "Учитель").
            string role = _context.AuthUsers.Any() ? Input.SelectedRole : "Админ";

            // Создаем нового пользователя (Обратите внимание: для продакшена пароль должен быть хэширован)
            var user = new AuthUser
            {
                LastName = Input.LastName,
                FirstName = Input.FirstName,
                MiddleName = Input.MiddleName,
                Email = Input.Email,
                Password = Input.Password,
                Role = role
            };

            _context.AuthUsers.Add(user);
            await _context.SaveChangesAsync();

            await Authenticate(user);
            return RedirectToPage("/Index");
        }

        private async Task Authenticate(AuthUser user)
        {
            var claims = new[]
            {
                // Если хотите, чтобы в Navigation выводилось только имя, передавайте user.FirstName:
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
