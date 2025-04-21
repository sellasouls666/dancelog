using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dancelog.Data;
using dancelog.Models;

namespace dancelog.Pages
{
    [Authorize(Roles = "Админ,Учитель")]
    public class LessonModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public LessonModel(ApplicationDbContext ctx) => _context = ctx;

        [BindProperty]
        public Lesson Lesson { get; set; } = new Lesson();

        [BindProperty]
        public DateTime LessonDate { get; set; }

        [BindProperty]
        public string SelectedTimeSlot { get; set; }

        [BindProperty]
        public TimeSpan? ExtraTime { get; set; }

        [BindProperty(SupportsGet = true, Name = "groupId")]
        public int? GroupIdFromQuery { get; set; }

        [BindProperty(SupportsGet = true, Name = "lessonDate")]
        public DateTime? DateFromQuery { get; set; }

        [BindProperty(SupportsGet = true, Name = "timeSlot")]
        public string SlotFromQuery { get; set; }

        public SelectList Groups { get; set; }
        public bool IsNew { get; set; }

        public async Task OnGetAsync(int? id)
        {
            IsNew = !id.HasValue || id == 0;
            Groups = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name");

            if (IsNew)
            {
                // 1) Если пришёл groupId в query — подставляем его
                if (GroupIdFromQuery.HasValue)
                    Lesson.GroupId = GroupIdFromQuery.Value;

                // 2) Если пришла дата — ставим её
                LessonDate = DateFromQuery ?? DateTime.Today;

                // 3) Слот из query
                if (!string.IsNullOrEmpty(SlotFromQuery))
                    SelectedTimeSlot = SlotFromQuery;
            }
            else
            {
                // логика редактирования существующего занятия…
                var e = await _context.Lessons.FindAsync(id);
                Lesson = e;
                LessonDate = e.DateTime.Date;
                var t = e.DateTime.TimeOfDay;
                if (t >= TimeSpan.Parse("08:00") && t <= TimeSpan.Parse("09:35")) SelectedTimeSlot = "First";
                else if (t >= TimeSpan.Parse("09:45") && t <= TimeSpan.Parse("11:20")) SelectedTimeSlot = "Second";
                else if (t >= TimeSpan.Parse("11:50") && t <= TimeSpan.Parse("13:25")) SelectedTimeSlot = "Third";
                else if (t >= TimeSpan.Parse("13:35") && t <= TimeSpan.Parse("15:10")) SelectedTimeSlot = "Fourth";
                else { SelectedTimeSlot = "Extra"; ExtraTime = t; }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Groups = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name");
            if (!ModelState.IsValid) return Page();

            var time = SelectedTimeSlot switch
            {
                "First" => TimeSpan.Parse("08:00"),
                "Second" => TimeSpan.Parse("09:45"),
                "Third" => TimeSpan.Parse("11:50"),
                "Fourth" => TimeSpan.Parse("13:35"),
                "Extra" => ExtraTime ?? throw new InvalidOperationException("Укажите время"),
                _ => throw new InvalidOperationException("Выберите слот")
            };
            Lesson.DateTime = LessonDate.Date + time;

            if (Lesson.Id == 0) _context.Lessons.Add(Lesson);
            else _context.Attach(Lesson).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return RedirectToPage("./ListOfLessons",
                new { SelectedGroupId = Lesson.GroupId });
        }
    }
}
