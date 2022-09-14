using Microsoft.EntityFrameworkCore;
using StudentTrackingApplicationReal.Shared.Models;

namespace StudentTrackingApplicationReal.Shared.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {}
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<StudentCourse> StudentsCourses { get; set; }
        public DbSet<SchoolUser> SchoolUsers { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Manager> Manager { get; set; }
    }
}
