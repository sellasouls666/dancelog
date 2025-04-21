using dancelog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dancelog.Pages
{
    public class LessonDeleteModel : PageModel
    {

            private readonly ApplicationDbContext _context;

            public LessonDeleteModel(ApplicationDbContext context)
            {
                _context = context;
            }

            public IActionResult OnGet(int id)
            {
                var group = _context.Lessons.FirstOrDefault(g => g.Id == id);
                if (group != null)
                {
                    _context.Lessons.Remove(group);
                    _context.SaveChanges();
                }
                return RedirectToPage("ListOfLessons");
            }
    }
}


