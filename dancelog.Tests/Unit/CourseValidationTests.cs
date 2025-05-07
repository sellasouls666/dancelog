using dancelog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dancelog.Tests.Unit
{
    public class CourseValidationTests
    {
        [Fact]
        public void Course_WithValidData_ShouldBeValid()
        {
            var course = new Course
            {
                Id = 0,
                Name = "3"
            };

            var context = new ValidationContext(course);

            // Сюда будут записаны ошибки валидации, если они есть
            var result = new List<ValidationResult>();

            // Проводим валидацию объекта с учетом всех атрибутов [Required], [Range] и т.п.
            var isValid = Validator.TryValidateObject(course, context, result, true);

            // Ожидаем, что валидация прошла успешно (все поля корректны)
            Assert.True(isValid);

            // Также убеждаемся, что список ошибок пуст
            Assert.Empty(result);
        }

        [Fact]
        public void Course_WithEmptyName_ShouldBeInvalid()
        {
            var course = new Course
            {
                Id = 0,
                Name = ""
            };

            var context = new ValidationContext(course);
            var result = new List<ValidationResult>();

            // Выполняем валидацию
            var isValid = Validator.TryValidateObject(course, context, result, true);

            // Ожидаем, что объект не прошел валидацию
            Assert.False(isValid);

            // Проверяем, что ошибка касается конкретно заголовка
            Assert.Contains(result, r => r.ErrorMessage == "Пожалуйста, введите название курса.");
        }
    }
}
