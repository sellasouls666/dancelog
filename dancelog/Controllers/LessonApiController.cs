using dancelog.Data;
using Microsoft.AspNetCore.Mvc;
using dancelog.Models;

namespace dancelog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LessonController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetLessons()
        {
            var lessons = _context.Lessons.ToList();
            return Ok(lessons);
        }

        [HttpGet("{id}")]
        public IActionResult GetLesson(int id)
        {
            var lesson = _context.Lessons.Find(id);
            if (lesson == null)
            {
                return NotFound();
            }
            return Ok(lesson);
        }

        [HttpPost]
        public IActionResult CreateLesson([FromBody] Lesson lesson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Lessons.Add(lesson);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetLesson), new { id = lesson.Id }, lesson);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateLesson(int id, [FromBody] Lesson lesson)
        {
            if (id != lesson.Id)
            {
                return BadRequest("ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingLesson = _context.Lessons.Find(id);
            if (existingLesson == null)
            {
                return NotFound();
            }

            existingLesson.Name = lesson.Name;
            existingLesson.GroupId = lesson.GroupId;
            existingLesson.Group = lesson.Group;
            existingLesson.DateTime = lesson.DateTime;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLesson(int id)
        {
            var lesson = _context.Lessons.Find(id);
            if (lesson == null)
            {
                return NotFound();
            }

            _context.Lessons.Remove(lesson);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
