using StudentTrackingApplicationBackEnd.Infrastructure;
using StudentTrackingApplicationReal.Shared.Models;
using System.Linq;

namespace StudentTrackingApplicationBackEnd.Services
{
    public class CourseRepo : Repository<Course>, ICourseRepo
    {

        public CourseRepo(Context dbContext) : base(dbContext)
        {}

        public IEnumerable<Course> GetTopCourses()
        {
            return dbContext.Courses.Take(2);
        }
    }
}
