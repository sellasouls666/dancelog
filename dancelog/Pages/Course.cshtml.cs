using dancelog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dancelog.Models;
using System;
using dancelog.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace dancelog.Pages
{
    public class CourseModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public CourseModel(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
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

        public async Task<IActionResult> OnPost()
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
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("CourseUpdated", Course);

            return RedirectToPage("ListOfCourse");
        }
    }
}
