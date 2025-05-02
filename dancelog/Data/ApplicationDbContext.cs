using Microsoft.EntityFrameworkCore;
using dancelog.Models;
using dancelog.Models.Auth;

namespace dancelog.Data
{
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {

            }

            public DbSet<Student> Students { get; set; }
            public virtual DbSet<Course> Courses { get; set; }
            public DbSet<Group> Groups { get; set; }
            public DbSet<Lesson> Lessons { get; set; }
            public DbSet<Attendance> AttendanceRecords { get; set; }
            public DbSet<AuthUser> AuthUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasOne(s => s.AuthUser)
                .WithOne(a => a.Student)
                .HasForeignKey<Student>(s => s.AuthUserId)
                .IsRequired();

        }
    }
}
