using dancelog.Data;
using dancelog.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace dancelog.Pages
{
    public class ListOfGroupsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ListOfGroupsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Group> Groups { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Groups = await _context.Groups.Include(g => g.Course).ToListAsync();
        }
    }
}