using dancelog.Data;
using dancelog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace dancelog.Pages
{
    public class ListOfCourseModel(ApplicationDbContext context) : PageModel
    {
        private readonly ApplicationDbContext _context = context;

        public List<Course> Courses { get; set; } = [];
        public void OnGet()
        {
            Courses = SampleData.GetCourses();
        }

        public IActionResult OnPostDelete(int Id)
        {
            var course = _context.Courses.Find(Id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
            return RedirectToPage();
        }
    }
}
