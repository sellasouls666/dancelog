using Microsoft.AspNetCore.Mvc;
using dancelogMVC.Models;

namespace dancelogMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesApiController : Controller
    {
        private static readonly List<Course> courses = new()
        {
            new Course { Id = 1, Name = "Test" }
        };

        [HttpGet]
        public IActionResult Get() => Ok(courses);

        [HttpPost]
        public IActionResult Post(Course course)
        {
            course.Id = courses.Max(c => c.Id) + 1;
            courses.Add(course);
            return CreatedAtAction(nameof(Get), new { id = course.Id }, course);
        }
    }
}
