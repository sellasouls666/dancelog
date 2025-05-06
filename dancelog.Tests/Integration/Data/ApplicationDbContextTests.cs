using Microsoft.EntityFrameworkCore; // Используется для работы с Entity Framework Core.
using dancelog.Data; // Контекст базы данных.
using dancelog.Models.Auth; // Модели для аутентификации.
using dancelog.Models;

namespace dancelog.Tests.Integration.Data
{
    public class ApplicationDbContextTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid()) // Уникальное имя базы данных для каждого теста
                .Options;

            // Создаём и возвращаем новый экземпляр контекста с заданными параметрами
            return new ApplicationDbContext(options);
        }

        [Fact]
        public void CanInsertAndReadEntities()
        {
            // Arrange: подготовка данных
            var context = GetDbContext(); // Создание контекста базы данных

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

            var lesson = new Lesson
            {
                Id = 0,
                Name = "MDK 01.01",
                GroupId = group.Id,
                Group = group,
                DateTime = DateTime.Now
            };

            var attendance = new Attendance
            {
                Id = 0,
                Lesson = lesson,
                Student = student,
                Status = "Присутствовал"
            };

            // Act: выполнение операции
            context.Courses.Add(course);
            context.Groups.Add(group);
            context.Students.Add(student); // Добавление студента в контекст
            context.Lessons.Add(lesson);
            context.AttendanceRecords.Add(attendance);
            context.SaveChanges(); // Сохранение изменений в базе данных

            // Assert: проверка результатов
            // Проверка, что все добавленные сущности сохранились в базе данных
            Assert.Single(context.Courses);
            Assert.Single(context.Groups);
            Assert.Single(context.Students); // Должен быть ровно один студент в базе
            Assert.Single(context.Lessons);
            Assert.Single(context.AttendanceRecords);

            var savedAttendanceRecord = context.AttendanceRecords.Include(a => a.Lesson).Include(a => a.Student).FirstOrDefault();
            Assert.NotNull(savedAttendanceRecord);
            Assert.Equal("MDK 01.01", savedAttendanceRecord.Lesson.Name);
            Assert.Equal("Глагольев", savedAttendanceRecord.Student.Surname);
        }
    }
}
