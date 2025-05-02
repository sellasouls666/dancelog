using dancelog.Data;
using dancelog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dancelog.Pages
{
    public class GroupModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GroupModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Group Group { get; set; }

        [BindProperty]
        public int? SelectedCourseId { get; set; }

        public SelectList CourseOptions { get; set; }

        public async Task OnGet(int id)
        {
            // Load courses for the dropdown
            var courses = await _context.Courses.ToListAsync();
            CourseOptions = new SelectList(courses, "Id", "Name");

            if (id > 0)
            {
                Group = await _context.Groups.Include(g => g.Course).FirstOrDefaultAsync(g => g.Id == id);
                if (Group != null)
                {
                    SelectedCourseId = Group.CourseId;
                }

            }
            else
            {
                Group = new Group() { Name = "" };
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Reload courses for the dropdown if validation fails
                var courses = await _context.Courses.ToListAsync();
                CourseOptions = new SelectList(courses, "Id", "Name");
                return Page();
            }

            // Fetch the selected Course from the database
            var selectedCourse = await _context.Courses.FindAsync(Group.CourseId);
            if (selectedCourse == null)
            {
                // Reload courses for the dropdown and display an error if the course isn't found
                var courses = await _context.Courses.ToListAsync();
                CourseOptions = new SelectList(courses, "Id", "Name");
                ModelState.AddModelError("Group.CourseId", "Выбранный курс не найден.");
                return Page();
            }

            if (Group.Id == 0)
            {
                Group.Course = selectedCourse;
                _context.Groups.Add(Group);
            }
            else
            {
                var groupToUpdate = await _context.Groups.Include(g => g.Course).FirstOrDefaultAsync(g => g.Id == Group.Id);
                if (groupToUpdate != null)
                {
                    groupToUpdate.Name = Group.Name;
                    groupToUpdate.Course = selectedCourse;
                    groupToUpdate.CourseId = Group.CourseId; // Ensure the CourseId is also updated
                    _context.Groups.Update(groupToUpdate);
                }
                else
                {
                    return NotFound(); // Handle the case where the group to update wasn't found
                }


            }

            await _context.SaveChangesAsync();
            return RedirectToPage("ListOfGroups");
        }
    }
}