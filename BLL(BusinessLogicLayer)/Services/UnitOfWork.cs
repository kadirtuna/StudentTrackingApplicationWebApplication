using StudentTrackingApplicationBackEnd.Infrastructure;
using StudentTrackingApplicationReal.Shared.Models;

namespace StudentTrackingApplicationBackEnd.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private Context _dbContext;
        public IAttendanceRepo Attendances { get; private set; }
        public ICourseRepo Courses { get; private set; }
        public IStudentRepo Students { get; private set; }
        public ISchoolUserRepo SchoolUsers { get; private set; }
        public IStudentCourseRepo StudentsCourses { get; private set; }
        public ITeacherRepo Teachers { get; private set; }
        public IManagerRepo Managers { get; private set; }


        public UnitOfWork(Context dbContext)
        {
            _dbContext = dbContext;
            Attendances = new AttendanceRepo(_dbContext);
            Courses = new CourseRepo(_dbContext);
            Students = new StudentRepo(_dbContext);
            StudentsCourses = new StudentCourseRepo(_dbContext);
            SchoolUsers = new SchoolUserRepo(_dbContext);
            Teachers = new TeacherRepo(_dbContext);
            Managers = new ManagerRepo(_dbContext);     
        }

        public int Complete()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
