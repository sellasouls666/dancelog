using dancelog.Data;
using Microsoft.AspNetCore.Mvc;
using dancelog.Models;

namespace dancelog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GroupController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetGroups()
        {
            var groups = _context.Groups.ToList();
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public IActionResult GetGroup(int id)
        {
            var group = _context.Groups.Find(id);
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group);
        }

        [HttpPost]
        public IActionResult CreateGroup([FromBody] Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Groups.Add(group);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetGroup), new { id = group.Id }, group);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGroup(int id, [FromBody] Group group)
        {
            if (id != group.Id)
            {
                return BadRequest("ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingGroup = _context.Groups.Find(id);
            if (existingGroup == null)
            {
                return NotFound();
            }

            existingGroup.Name = group.Name;
            existingGroup.CourseId = group.CourseId;
            existingGroup.Course = group.Course;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGroup(int id)
        {
            var group = _context.Groups.Find(id);
            if (group == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(group);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
