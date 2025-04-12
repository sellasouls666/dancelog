using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dancelog.Data;
using dancelog.Models.Auth;
using System.Linq;

namespace dancelog.Pages.Account
{
    [Authorize]
    public class ProfilePageModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProfilePageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Для передачи информации о пользователе в представление
        public AuthUser CurrentUser { get; set; }

        public IActionResult OnGet()
        {
            // Получаем email из claim пользователя
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToPage("/Account/Login");
            }

            // Ищем пользователя в базе по email
            CurrentUser = _context.AuthUsers.FirstOrDefault(u => u.Email == email);
            if (CurrentUser == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
