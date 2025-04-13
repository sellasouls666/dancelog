using dancelog.Data;
using dancelog.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dancelog.Pages.Account
{
    public class UserModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public UserModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AuthUser User { get; set; }

        public SelectList RoleOptions { get; set; }

        public async Task OnGetAsync(int id)
        {
            // Загружаем роли для выпадающего списка
            var roles = new List<string> { "Админ", "Учитель", "Ученик" };
            RoleOptions = new SelectList(roles);

            User = id == 0 ? new AuthUser() : await _context.AuthUsers.FindAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Перезагружаем роли при ошибке валидации
                var roles = new List<string> { "Админ", "Учитель", "Ученик" };
                RoleOptions = new SelectList(roles);
                return Page();
            }

            if (User.Id == 0)
            {
                // Добавляем нового пользователя
                _context.AuthUsers.Add(User);
            }
            else
            {
                // Обновляем существующего пользователя
                _context.AuthUsers.Update(User);
            }

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("ListOfUsers");
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", $"Ошибка при сохранении: {ex.Message}");
                var roles = new List<string> { "Админ", "Учитель", "Ученик" };
                RoleOptions = new SelectList(roles);
                return Page();
            }
        }
    }
}