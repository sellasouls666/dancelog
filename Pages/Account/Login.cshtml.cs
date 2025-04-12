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
    public class LoginPageModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginPageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Свойство для привязки данных из формы
        [BindProperty]
        public LoginModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Ищем пользователя по Email и Password
            var user = _context.AuthUsers.FirstOrDefault(u => u.Email == Input.Email && u.Password == Input.Password);
            if (user != null)
            {
                await Authenticate(user);
                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Неверный Email или пароль");
            return Page();
        }

        private async Task Authenticate(AuthUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.FullName), // отображается ФИО, а не email
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
