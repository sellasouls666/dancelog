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
    public class CourseModelTest
    {
        // Метод создает In-Memory базу данных EF Core с тестовыми данными
        private ApplicationDbContext GetDbContext()
        {
            // Создаем уникальную базу данных в памяти для каждого теста
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // важно: уникальное имя
                .Options;

            // Создаем контекст БД
            var context = new ApplicationDbContext(options);

            // Добавляем тестовые книги
            context.Courses.AddRange(
                new Course { Id = 1, Name = "1" },
                new Course { Id = 2, Name = "2" }
            );

            // Сохраняем изменения в базу
            context.SaveChanges();

            return context; // возвращаем контекст с предзаполненными данными
        }

        [Fact]
        public void OnGet_ShouldLoadCourses()
        {
            // Arrange — подготавливаем тестовые данные и модель
            var context = GetDbContext();          // получаем фейковый контекст с книгами
            var model = new ListOfCourseModel(context);   // создаем экземпляр модели страницы

            // Act — вызываем метод OnGet, который должен заполнить список книг
            model.OnGet();

            // Assert — проверяем, что результат соответствует ожиданиям
            Assert.NotNull(model.Courses);                 // список должен быть не null
            Assert.Equal(2, model.Courses.Count);          // в списке должно быть ровно 2 книги
            Assert.Contains(model.Courses, b => b.Id == 1);  // проверка по названию
            Assert.Contains(model.Courses, b => b.Name == "2"); // проверка по автору
        }
    }
}
