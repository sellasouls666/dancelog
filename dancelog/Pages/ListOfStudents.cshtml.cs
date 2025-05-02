using dancelog.Data;
using dancelog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace dancelog.Pages
{
    public class ListOfStudentsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ListOfStudentsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Student> Students { get; set; }

        public async Task OnGetAsync()
        {
            Students = await _context.Students
                .Include(s => s.Group)
                .ToListAsync();
        }
    }
}