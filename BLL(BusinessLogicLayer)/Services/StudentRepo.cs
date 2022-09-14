using StudentTrackingApplicationBackEnd.Infrastructure;
using StudentTrackingApplicationReal.Shared.Models;

namespace StudentTrackingApplicationBackEnd.Services
{
    public class StudentRepo : Repository<Student>, IStudentRepo
    {
        public StudentRepo(Context context) : base(context)
        {
        }
    }
}
