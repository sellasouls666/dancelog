using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dancelog.Data;
using dancelog.Models;

namespace dancelog.Pages
{
    [Authorize]
    public class ListOfLessonsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ListOfLessonsModel(ApplicationDbContext ctx) => _context = ctx;

        public SelectList Groups { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedGroupId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime WeekStart { get; set; }

        public string WeekStartString => WeekStart.ToString("yyyy-MM-dd");

        public class TimeSlot
        {
            public string Key { get; set; }
            public string Display { get; set; }
            public TimeSpan Time { get; set; }
        }

        public List<TimeSlot> TimeSlots { get; } = new()
        {
            new() { Key="First",  Display="8:00–9:35",   Time=new TimeSpan(8,0,0)  },
            new() { Key="Second", Display="9:45–11:20",  Time=new TimeSpan(9,45,0) },
            new() { Key="Third",  Display="11:50–13:25", Time=new TimeSpan(11,50,0)},
            new() { Key="Fourth", Display="13:35–15:10", Time=new TimeSpan(13,35,0)}
        };

        public List<Lesson> Lessons { get; set; } = new();

        public async Task OnGetAsync()
        {
            var allGroups = await _context.Groups.ToListAsync();
            Groups = new SelectList(allGroups, "Id", "Name");

            // если нет group в query — попытаться взять из куки
            if (!SelectedGroupId.HasValue
                && Request.Cookies.TryGetValue("LastGroup", out var c)
                && int.TryParse(c, out var lg)
                && allGroups.Any(g => g.Id == lg))
            {
                SelectedGroupId = lg;
            }

            // сохранить выбор в куки
            if (SelectedGroupId.HasValue)
            {
                Response.Cookies.Append(
                    "LastGroup",
                    SelectedGroupId.Value.ToString(),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }

            if (WeekStart == default)
            {
                var today = DateTime.Today;
                int diff = ((int)today.DayOfWeek + 6) % 7;
                WeekStart = today.AddDays(-diff);
            }

            var q = _context.Lessons.Include(l => l.Group).AsQueryable();
            if (SelectedGroupId.HasValue)
                q = q.Where(l => l.GroupId == SelectedGroupId.Value);

            var weekEnd = WeekStart.AddDays(5).AddHours(23).AddMinutes(59);
            q = q.Where(l => l.DateTime >= WeekStart && l.DateTime <= weekEnd);

            Lessons = await q.ToListAsync();
        }
    }
}