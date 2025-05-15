using dancelog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dancelog.Tests.Unit
{
    public class LessonValidationTests
    {
        [Fact]
        public void Lesson_WithValidData_ShouldBeValid()
        {
            var course = new Course
            {
                Id = 0,
                Name = "3"
            };

            var group = new Group
            {
                Id = 0,
                Name = "П-30",
                CourseId = course.Id,
                Course = course
            };

            var lesson = new Lesson
            {
                Id = 0,
                Name = "MDK 01.01",
                GroupId = group.Id,
                Group = group,
                DateTime = DateTime.Now
            };

            var context = new ValidationContext(lesson);

            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(lesson, context, result, true);

            Assert.True(isValid);

            Assert.Empty(result);
        }

        [Fact]
        public void Lesson_WithEmptyName_ShouldBeInvalid()
        {
            var course = new Course
            {
                Id = 0,
                Name = "3"
            };

            var group = new Group
            {
                Id = 0,
                Name = "П-30",
                CourseId = course.Id,
                Course = course
            };

            var lesson = new Lesson
            {
                Id = 0,
                Name = "",
                GroupId = group.Id,
                Group = group,
                DateTime = DateTime.Now
            };

            var context = new ValidationContext(lesson);
            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(lesson, context, result, true);

            Assert.False(isValid);

            Assert.Contains(result, r => r.ErrorMessage == "Необходимо указать название урока");
        }
    }
}
