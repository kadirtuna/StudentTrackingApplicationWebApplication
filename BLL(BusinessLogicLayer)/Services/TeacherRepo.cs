using StudentTrackingApplicationBackEnd.Infrastructure;
using StudentTrackingApplicationReal.Shared.Models;

namespace StudentTrackingApplicationBackEnd.Services
{
    public class TeacherRepo : Repository<Teacher>, ITeacherRepo
    {
        public TeacherRepo(Context _dbContext) : base(_dbContext)
        {
        }
    }
}
