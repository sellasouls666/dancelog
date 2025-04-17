using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using dancelog.Models;
using Microsoft.EntityFrameworkCore;
using dancelog.Data;
using System.Text.RegularExpressions;
using Group = dancelog.Models.Group;

namespace dancelog.Pages
{
    public class StudentModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public StudentModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; }

        public SelectList Groups { get; set; }  // Список групп для выпадающего списка

        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Id = id;

            Groups = new SelectList(await _context.Groups
                .Include(g => g.Course)
                .OrderBy(g => g.Name)
                .ToListAsync(),
                nameof(Group.Id),
                nameof(Group.Name));

            if (id == 0)
            {
                Student = new Student
                {
                    Surname = string.Empty,
                    Name = string.Empty,
                    PhoneNumber = string.Empty
                };
                return Page();
            }

            Student = await _context.Students
                .Include(s => s.Group)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (Student == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!Regex.IsMatch(Student.PhoneNumber, @"^\d{10,11}$"))
            {
                ModelState.AddModelError("Student.PhoneNumber", "Номер телефона должен содержать 10-11 цифр");
                Groups = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                // Перезагружаем группы при ошибке
                Groups = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name");
                return Page();
            }

            // Проверяем существование группы
            var groupExists = await _context.Groups.AnyAsync(g => g.Id == Student.GroupId);
            if (!groupExists)
            {
                ModelState.AddModelError("Student.GroupId", "Выбранная группа не существует");
                Groups = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name");
                return Page();
            }

            if (Student.Id == 0)
            {
                _context.Students.Add(Student);
            }
            else
            {
                _context.Attach(Student).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("ListOfStudents");
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Не удалось сохранить данные: " + ex.Message);
                Groups = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name");
                return Page();
            }
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}