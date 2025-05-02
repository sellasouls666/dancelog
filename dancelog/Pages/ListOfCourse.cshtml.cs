using dancelog.Data;
using dancelog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace dancelog.Pages
{
    public class ListOfCourseModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<Course> Courses { get; set; }

        public ListOfCourseModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Courses = _context.Courses.ToList();
        }
    }
}
