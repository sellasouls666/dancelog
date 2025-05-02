using dancelog.Data;
using dancelog.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dancelog.Pages.Account
{
    public class UserDeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public UserDeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            var user = _context.AuthUsers.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.AuthUsers.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToPage("ListOfUsers");
        }
    }
}