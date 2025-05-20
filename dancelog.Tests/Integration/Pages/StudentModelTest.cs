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
    public class StudentModelTest
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

            context.Students.AddRange(
                    new Student { Id = 3, Surname = "Глагольев", Name = "Егор", Patronymic = "Викторович", GroupId = 2, Email = "pgbc303@gmail.com"},
                    new Student { Id = 4, Surname = "Цветков", Name = "Егор", Patronymic = "Денисович", GroupId = 2, Email = "egortsvetkov@mail.ru" }
                );

            context.SaveChanges();

            return context;
        }

        [Fact]
        public void OnGet_ShouldLoadStudents()
        {
            var context = GetDbContext();
            var model = new ListOfStudentsModel(context);

            model.OnGetAsync();

            Assert.NotNull(model.Students);
            Assert.Equal(2, model.Students.Count);
            Assert.Contains(model.Students, s => s.Id == 3);
            Assert.Contains(model.Students, s => s.Surname == "Цветков");
        }
    }
}
