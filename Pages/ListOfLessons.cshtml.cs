using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using dancelog.Models;
using dancelog.Data;
using Microsoft.AspNetCore.Authorization;

namespace dancelog.Pages
{
    [Authorize]
    public class ListOfLessonsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ListOfLessonsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Lesson>? Lessons { get; set; }

        public async Task OnGetAsync()
        {
            Lessons = await _context.Lessons.Include(l => l.Group).ToListAsync();
        }
    }
}