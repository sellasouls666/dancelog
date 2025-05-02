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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsNew = !id.HasValue || id == 0;
            Groups = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name");

            if (IsNew)
            {
                if (GroupIdFromQuery.HasValue)
                    Lesson.GroupId = GroupIdFromQuery.Value;

                LessonDate = DateFromQuery ?? DateTime.Today;

                if (!string.IsNullOrEmpty(SlotFromQuery))
                    SelectedTimeSlot = SlotFromQuery;
            }
            else
            {
                Lesson = await _context.Lessons
                    .Include(l => l.Group)
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (Lesson == null)
                {
                    return NotFound();
                }

                LessonDate = Lesson.DateTime.Date;
                var time = Lesson.DateTime.TimeOfDay;

                // Более точное определение слота
                if (time >= TimeSpan.Parse("08:00") && time < TimeSpan.Parse("09:45"))
                    SelectedTimeSlot = "First";
                else if (time >= TimeSpan.Parse("09:45") && time < TimeSpan.Parse("11:50"))
                    SelectedTimeSlot = "Second";
                else if (time >= TimeSpan.Parse("11:50") && time < TimeSpan.Parse("13:35"))
                    SelectedTimeSlot = "Third";
                else if (time >= TimeSpan.Parse("13:35") && time < TimeSpan.Parse("15:10"))
                    SelectedTimeSlot = "Fourth";
                else
                {
                    SelectedTimeSlot = "Extra";
                    ExtraTime = time;
                }
            }

            return Page();
        }

        private async Task LoadGroups()
        {
            Groups = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Загружаем группы для повторного отображения формы при ошибке
            Groups = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name");

            // Проверяем только основные поля урока
            if (string.IsNullOrEmpty(Lesson.Name) || Lesson.GroupId == 0)
            {
                ModelState.AddModelError("", "Заполните обязательные поля");
                return Page();
            }

            try
            {
                // Обработка времени
                TimeSpan time;
                if (SelectedTimeSlot == "Extra")
                {
                    if (!ExtraTime.HasValue)
                    {
                        ModelState.AddModelError("ExtraTime", "Укажите время для доп. урока");
                        return Page();
                    }
                    time = ExtraTime.Value;
                }
                else
                {
                    time = SelectedTimeSlot switch
                    {
                        "First" => TimeSpan.Parse("08:00"),
                        "Second" => TimeSpan.Parse("09:45"),
                        "Third" => TimeSpan.Parse("11:50"),
                        "Fourth" => TimeSpan.Parse("13:35"),
                        _ => throw new InvalidOperationException("Выберите временной слот")
                    };
                }

                Lesson.DateTime = LessonDate.Date + time;

                if (Lesson.Id == 0)
                {
                    _context.Lessons.Add(Lesson);
                }
                else
                {
                    var existingLesson = await _context.Lessons.FindAsync(Lesson.Id);
                    if (existingLesson == null)
                    {
                        return NotFound();
                    }

                    existingLesson.Name = Lesson.Name;
                    existingLesson.GroupId = Lesson.GroupId;
                    existingLesson.DateTime = Lesson.DateTime;
                }

                await _context.SaveChangesAsync();
                return RedirectToPage("./ListOfLessons", new { SelectedGroupId = Lesson.GroupId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка сохранения: {ex.Message}");
                return Page();
            }
        }
    }
    }

