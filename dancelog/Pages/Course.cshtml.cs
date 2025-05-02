using dancelog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dancelog.Models;
using System;

namespace dancelog.Pages
{
    public class CourseModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CourseModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course Course { get; set; }

        public void OnGet(int id)
        {
            if (id > 0)
            {
                Course = _context.Courses.FirstOrDefault(c => c.Id == id);
            }
            else
            {
                Course = new Course() { Name = ""};
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Course.Id == 0)
            {
                _context.Courses.Add(Course);
            }
            else
            {
                _context.Courses.Update(Course);
            }
            _context.SaveChanges();
            return RedirectToPage("ListOfCourse");
        }
    }
}
