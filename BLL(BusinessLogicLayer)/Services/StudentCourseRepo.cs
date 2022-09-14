using StudentTrackingApplicationBackEnd.Infrastructure;
using StudentTrackingApplicationReal.Shared.Models;
namespace StudentTrackingApplicationBackEnd.Services
{
    public class StudentCourseRepo : Repository<StudentCourse>, IStudentCourseRepo
    {
        public StudentCourseRepo(Context _dbContext) : base(_dbContext)
        {
        }
    }
}
