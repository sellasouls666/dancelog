using dancelog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dancelog.Pages
{
    public class GroupDeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GroupDeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            var group = _context.Groups.FirstOrDefault(g => g.Id == id);
            if (group != null)
            {
                _context.Groups.Remove(group);
                _context.SaveChanges();
            }
            return RedirectToPage("ListOfGroups");
        }
    }
}
