using Microsoft.EntityFrameworkCore;
using dancelog.Data;
using dancelog.Models;
using dancelog.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dancelog.Tests.Integration.Pages
{
    public class LessonModelTest
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);

            context.Courses.Add(
                new Course { Id = 1, Name = "1" }
            );

            context.Groups.Add(
                new Group { Id = 2, Name = "П-10", CourseId = 1 }
            );

            context.Lessons.AddRange(
                    new Lesson { Id = 3, Name = "МДК 01.01", GroupId = 2, DateTime = DateTime.Now },
                    new Lesson { Id = 4, Name = "МДК 01.02", GroupId = 2, DateTime = DateTime.Now }
                );

            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task OnGet_ShouldLoadLessons()
        {
            var context = GetDbContext();
            var model = new ListOfLessonsModel(context)
            {
                SelectedGroupId = 2, 
                WeekStart = DateTime.Today 
            };

            model.PageContext = new PageContext
            {
                HttpContext = new DefaultHttpContext()
            };

            await model.OnGetAsync();

            Assert.NotNull(model.Lessons);
            Assert.Equal(2, model.Lessons.Count);
            Assert.Contains(model.Lessons, l => l.Id == 3);
            Assert.Contains(model.Lessons, l => l.Name == "МДК 01.02");
        }
    }
}
