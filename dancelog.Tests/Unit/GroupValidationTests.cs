using dancelog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dancelog.Tests.Unit
{
    public class GroupValidationTests
    {
        [Fact]
        public void Group_WithValidData_ShouldBeValid()
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

            var context = new ValidationContext(group);

            // Сюда будут записаны ошибки валидации, если они есть
            var result = new List<ValidationResult>();

            // Проводим валидацию объекта с учетом всех атрибутов [Required], [Range] и т.п.
            var isValid = Validator.TryValidateObject(group, context, result, true);

            // Ожидаем, что валидация прошла успешно (все поля корректны)
            Assert.True(isValid);

            // Также убеждаемся, что список ошибок пуст
            Assert.Empty(result);
        }

        [Fact]
        public void Group_WithEmptyName_ShouldBeInvalid()
        {
            var course = new Course
            {
                Id = 0,
                Name = "3"
            };

            var group = new Group
            {
                Id = 0,
                Name = "",
                CourseId = course.Id,
                Course = course
            };

            var context = new ValidationContext(group);
            var result = new List<ValidationResult>();

            // Выполняем валидацию
            var isValid = Validator.TryValidateObject(group, context, result, true);

            // Ожидаем, что объект не прошел валидацию
            Assert.False(isValid);

            // Проверяем, что ошибка касается конкретно заголовка
            Assert.Contains(result, r => r.ErrorMessage == "Пожалуйста, введите название группы.");
        }
    }
}
