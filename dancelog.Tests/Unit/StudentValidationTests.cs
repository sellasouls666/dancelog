using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dancelog.Models;

namespace dancelog.Tests.Unit
{
    public class StudentValidationTests
    {
        [Fact]
        public void Student_WithValidData_ShouldBeValid()
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

            var student = new Student
            {
                Id = 0,
                Surname = "Глагольев",
                Name = "Егор",
                Patronymic = "Викторович",
                GroupId = group.Id,
                Group = group,
                Email = "pgbc303@gmail.com"
            };

            var context = new ValidationContext(student);

            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(student, context, result, true);

            Assert.True(isValid);

            Assert.Empty(result);
        }

        [Fact]
        public void Student_WithEmptySurname_ShouldBeInvalid()
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

            var student = new Student
            {
                Id = 0,
                Surname = "",
                Name = "Егор",
                Patronymic = "Викторович",
                GroupId = group.Id,
                Group = group,
                Email = "pgbc303@gmail.com"
            };

            var context = new ValidationContext(student);
            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(student, context, result, true);

            Assert.False(isValid);

            Assert.Contains(result, r => r.ErrorMessage == "Поле обязательно для заполнения");
        }
    }
}
