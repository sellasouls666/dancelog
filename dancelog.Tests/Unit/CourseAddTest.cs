using dancelog.Pages;
using dancelog.Data;
using dancelog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace dancelog.Tests
{
    public class CourseAddTest
    {
        [Fact]
        public void OnPost_AddsNewCourse_WhenModelIsValid()
        {
            // Arrange
            // 1. Создаем мок DbSet<Course>
            var mockSet = new Mock<DbSet<Course>>();

            // 2. Создаем мок DbContextOptions
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .Options;

            // 3. Создаем мок ApplicationDbContext с передачей options
            var mockContext = new Mock<ApplicationDbContext>(options);

            // 4. Настраиваем виртуальное свойство Courses
            mockContext.Setup(m => m.Courses).Returns(mockSet.Object);

            // 5. Настраиваем SaveChanges
            mockContext.Setup(m => m.SaveChanges()).Returns(1);

            var model = new CourseModel(mockContext.Object)
            {
                Course = new Course { Name = "Test Course" }
            };

            // Act
            var result = model.OnPost();

            // Assert
            mockSet.Verify(m => m.Add(It.IsAny<Course>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}