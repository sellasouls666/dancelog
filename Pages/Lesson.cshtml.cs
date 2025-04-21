using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using dancelog.Models;
using dancelog.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace dancelog.Pages
{
    [Authorize]
    public class LessonModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LessonModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Lesson Lesson { get; set; } = new Lesson();

        public SelectList? Groups { get; set; } // Changed from Courses to Groups

        public bool IsNew { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsNew = !id.HasValue || id == 0;

            // Загрузка списка групп для выпадающего списка
            Groups = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name"); // Changed from Courses to Groups

            if (!IsNew)
            {
                Lesson = await _context.Lessons.Include(l => l.Group).FirstOrDefaultAsync(m => m.Id == id); // Changed from Course to Group

                if (Lesson == null)
                {
                    return NotFound();
                }
            }
            else
            {
                Lesson = new Lesson();
                Lesson.DateTime = DateTime.Now;
            }


            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Groups = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name"); // Changed from Courses to Groups
                return Page();
            }

            if (Lesson.Id == 0)
            {
                _context.Lessons.Add(Lesson);
            }
            else
            {
                _context.Attach(Lesson).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Ошибка при сохранении в базу данных: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Внутреннее исключение: {ex.InnerException.Message}");
                }
                ModelState.AddModelError("", "Произошла ошибка при сохранении данных. Пожалуйста, попробуйте еще раз.");
                Groups = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name"); // Changed from Courses to Groups
                return Page();
            }
            return RedirectToPage("./ListOfLessons");
        }

        private bool LessonExists(int id)
        {
            return _context.Lessons.Any(e => e.Id == id);
        }
    }
}