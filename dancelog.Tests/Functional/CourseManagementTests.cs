using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dancelog.Tests.Functional
{
    public class CourseManagementTests
    {
        private IWebDriver driver;

        public CourseManagementTests()
        {
            driver = new ChromeDriver();
        }

        [Fact]
        public void CanAddCourse()
        {
            // Arrange
            driver.Navigate().GoToUrl("http://localhost:5233/Course?id=0");

            // Act
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // Заполнение формы
            wait.Until(d => d.FindElement(By.Id("Course_Name"))).SendKeys("Б-20");
            driver.FindElement(By.XPath("//button[contains(text(),'Добавить')]")).Click();

            // Assert
            // Ждём появления именно нашего курса
            wait.Until(d => d.FindElements(By.CssSelector(".card-title"))
                .Any(title => title.Text == "Б-20"));

            // Альтернативная проверка
            var courseElement = driver.FindElement(
                By.XPath("//h5[@class='card-title' and text()='Б-20']"));
            Assert.NotNull(courseElement);
        }
    }
}
