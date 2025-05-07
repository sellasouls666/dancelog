using dancelog.Data;
using dancelog.Hubs;
using dancelog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace dancelog.Pages
{
    public class CourseDeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public CourseDeleteModel(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }


        public async Task<IActionResult> OnGet(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Id == id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("CourseDeleted", id);
            }

            return RedirectToPage("ListOfCourse");
        }
    }
}
