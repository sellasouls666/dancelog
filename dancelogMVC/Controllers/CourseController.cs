using Microsoft.AspNetCore.Mvc;
using dancelogMVC.Models;

namespace dancelogMVC.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {
            var courses = new List<Course> {
            new Course { Id = 1, Name = "Test" }};

            return View(courses);
        }
    }
}
