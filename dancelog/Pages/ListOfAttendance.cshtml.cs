using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dancelog.Data;
using dancelog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dancelog.Pages
{
    [Authorize]
    public class ListOfAttendanceModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ListOfAttendanceModel(ApplicationDbContext context) => _context = context;

        public SelectList Groups { get; set; } = null!;
        public SelectList DatesList { get; set; } = null!;
        public SelectList LessonsList { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public int? SelectedGroupId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedLessonId { get; set; }

        [BindProperty]
        public Dictionary<int, string> AttendanceStatuses { get; set; } = new();

        public List<AttendanceRow> AttendanceRows { get; set; } = new();

        public class AttendanceRow
        {
            public int StudentId { get; set; }
            public string FullName { get; set; } = string.Empty;
            public string Status { get; set; } = "Присутствовал";
        }

        public async Task OnGetAsync()
        {
            if (Request.Query.ContainsKey("SelectedGroupId") && SelectedGroupId == null)
            {
                Response.Cookies.Delete("LastAttendanceGroup");
                Response.Cookies.Delete("LastAttendanceDate");
                Response.Cookies.Delete("LastAttendanceLesson");
                SelectedDate = null;
                SelectedLessonId = null;
            }

            var allGroups = await _context.Groups
                .AsNoTracking()
                .OrderBy(g => g.Name)
                .ToListAsync();
            Groups = new SelectList(allGroups, "Id", "Name");

            if (!SelectedGroupId.HasValue
                && Request.Cookies.TryGetValue("LastAttendanceGroup", out var cg)
                && int.TryParse(cg, out var parsedGroup)
                && allGroups.Any(g => g.Id == parsedGroup))
            {
                SelectedGroupId = parsedGroup;
            }

            if (SelectedGroupId.HasValue)
            {
                Response.Cookies.Append(
                    "LastAttendanceGroup",
                    SelectedGroupId.Value.ToString(),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }

            List<DateTime> lessonDates = new();
            if (SelectedGroupId.HasValue)
            {
                lessonDates = await _context.Lessons
                    .Where(l => l.GroupId == SelectedGroupId.Value)
                    .Select(l => l.DateTime.Date)
                    .Distinct()
                    .OrderBy(d => d)
                    .ToListAsync();

                var dateItems = lessonDates
                    .Select(d => new
                    {
                        Value = d.ToString("yyyy-MM-dd"),
                        Text = d.ToString("dd.MM.yyyy")
                    })
                    .ToList();

                DatesList = new SelectList(dateItems, "Value", "Text");
            }

            if (Request.Query.ContainsKey("SelectedDate") && string.IsNullOrEmpty(SelectedDate))
            {
                Response.Cookies.Delete("LastAttendanceDate");
                Response.Cookies.Delete("LastAttendanceLesson");
                SelectedLessonId = null;
            }

            if (string.IsNullOrEmpty(SelectedDate)
                && Request.Cookies.TryGetValue("LastAttendanceDate", out var cd)
                && DateTime.TryParseExact(cd, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var parsedDate)
                && SelectedGroupId.HasValue
                && lessonDates.Contains(parsedDate.Date))
            {
                SelectedDate = cd;
            }

            if (!string.IsNullOrEmpty(SelectedDate))
            {
                Response.Cookies.Append(
                    "LastAttendanceDate",
                    SelectedDate,
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }

            List<Lesson> lessonsOnDate = new();
            DateTime selectedDateValue = default;
            if (SelectedGroupId.HasValue
                && !string.IsNullOrEmpty(SelectedDate)
                && DateTime.TryParseExact(
                       SelectedDate,
                       "yyyy-MM-dd",
                       null,
                       System.Globalization.DateTimeStyles.None,
                       out selectedDateValue))
            {
                lessonsOnDate = await _context.Lessons
                    .Where(l => l.GroupId == SelectedGroupId.Value
                                && l.DateTime.Date == selectedDateValue.Date)
                    .OrderBy(l => l.DateTime.TimeOfDay)
                    .ToListAsync();

                LessonsList = new SelectList(lessonsOnDate, "Id", "Name");
            }

            if (Request.Query.ContainsKey("SelectedLessonId") && SelectedLessonId == null)
            {
                Response.Cookies.Delete("LastAttendanceLesson");
            }

            if (!SelectedLessonId.HasValue
                && Request.Cookies.TryGetValue("LastAttendanceLesson", out var cl)
                && int.TryParse(cl, out var parsedLesson)
                && lessonsOnDate.Any(l => l.Id == parsedLesson))
            {
                SelectedLessonId = parsedLesson;
            }

            if (SelectedLessonId.HasValue)
            {
                Response.Cookies.Append(
                    "LastAttendanceLesson",
                    SelectedLessonId.Value.ToString(),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }

            if (SelectedLessonId.HasValue)
            {
                int lessonId = SelectedLessonId.Value;

                var students = await _context.Students
                    .Where(s => s.GroupId == SelectedGroupId.Value)
                    .OrderBy(s => s.Surname)
                    .ThenBy(s => s.Name)
                    .AsNoTracking()
                    .ToListAsync();

                var existingAttendances = await _context.AttendanceRecords
                    .Where(a => a.LessonId == lessonId)
                    .AsNoTracking()
                    .ToListAsync();

                AttendanceRows = students.Select(s =>
                {
                    var rec = existingAttendances.FirstOrDefault(a => a.StudentId == s.Id);
                    return new AttendanceRow
                    {
                        StudentId = s.Id,
                        FullName = $"{s.Surname} {s.Name} {s.Patronymic}".Trim(),
                        Status = rec?.Status ?? "Присутствовал"
                    };
                }).ToList();

                AttendanceStatuses = AttendanceRows.ToDictionary(
                    row => row.StudentId,
                    row => row.Status
                );
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!SelectedGroupId.HasValue
                || string.IsNullOrEmpty(SelectedDate)
                || !SelectedLessonId.HasValue)
            {
                return RedirectToPage(new
                {
                    SelectedGroupId,
                    SelectedDate = SelectedDate,
                    SelectedLessonId
                });
            }

            if (!DateTime.TryParseExact(
                    SelectedDate,
                    "yyyy-MM-dd",
                    null,
                    System.Globalization.DateTimeStyles.None,
                    out var parsedDateValue))
            {
                return RedirectToPage();
            }

            int lessonId = SelectedLessonId.Value;

            var existingAttendances = await _context.AttendanceRecords
                .Where(a => a.LessonId == lessonId)
                .ToListAsync();

            foreach (var kvp in AttendanceStatuses)
            {
                int studentId = kvp.Key;
                string newStatus = kvp.Value;

                var exist = existingAttendances.FirstOrDefault(a => a.StudentId == studentId);
                if (exist != null)
                {
                    if (exist.Status != newStatus)
                    {
                        exist.Status = newStatus;
                        _context.AttendanceRecords.Update(exist);
                    }
                }
                else
                {
                    _context.AttendanceRecords.Add(new Attendance
                    {
                        LessonId = lessonId,
                        StudentId = studentId,
                        Status = newStatus
                    });
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToPage(new
            {
                SelectedGroupId,
                SelectedDate = SelectedDate,
                SelectedLessonId
            });
        }
    }
}
