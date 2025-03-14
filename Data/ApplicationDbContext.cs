using Microsoft.EntityFrameworkCore;
using dancelog.Models;

namespace dancelog.Data
{
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
                Database.Migrate();
            }

            public DbSet<Student> Students { get; set; }
            public DbSet<Course> Courses { get; set; }
            public DbSet<Group> Groups { get; set; }
            public DbSet<Lesson> Lessons { get; set; }
            public DbSet<Attendance> AttendanceRecords { get; set; }

        }
}
