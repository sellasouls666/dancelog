using Microsoft.EntityFrameworkCore;
using dancelog.Data;
using dancelog.Models;
using dancelog.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dancelog.Tests.Integration.Pages
{
    public class GroupModelTest
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

            context.Groups.AddRange(
                new Group { Id = 2, Name = "П-10", CourseId = 1 },
                new Group { Id = 3, Name = "Б-10", CourseId = 1 }
            );

            context.SaveChanges();

            return context;
        }

        [Fact]
        public void OnGet_ShouldLoadGroups()
        {
            var context = GetDbContext();       
            var model = new ListOfGroupsModel(context);  

            model.OnGetAsync();

            Assert.NotNull(model.Groups);               
            Assert.Equal(2, model.Groups.Count);         
            Assert.Contains(model.Groups, b => b.Id == 2); 
            Assert.Contains(model.Groups, b => b.Name == "Б-10"); 
        }
    }
}
