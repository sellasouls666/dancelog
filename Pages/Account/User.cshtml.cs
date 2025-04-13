using dancelog.Data;
using dancelog.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        public async Task OnGetAsync(int id)
        {
            User = id == 0 ? new AuthUser() : await _context.AuthUsers.FindAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (User.Id == 0)
            {
                _context.AuthUsers.Add(User);
            }
            else
            {
                _context.AuthUsers.Update(User);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("ListOfUsers");
        }
    }
}