using StudentTrackingApplicationReal.Shared.Models;

namespace StudentTrackingApplicationBackEnd.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IAttendanceRepo Attendances { get; } 
        ICourseRepo Courses { get; }
        IStudentRepo Students { get; }
        IStudentCourseRepo StudentsCourses { get; }
        ISchoolUserRepo SchoolUsers { get; }
        ITeacherRepo Teachers { get; }
        IManagerRepo Managers { get; }

        int Complete();
    }
}
