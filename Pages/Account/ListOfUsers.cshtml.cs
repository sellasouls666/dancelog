using dancelog.Data;
using dancelog.Models.Auth;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace dancelog.Pages.Account
{
    public class ListOfUsersModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ListOfUsersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<AuthUser> Users { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Users = await _context.AuthUsers.ToListAsync();
        }
    }
}