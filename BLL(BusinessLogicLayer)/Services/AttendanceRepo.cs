using Microsoft.EntityFrameworkCore;
using StudentTrackingApplicationBackEnd.Infrastructure;
using StudentTrackingApplicationReal.Shared.Models;

namespace StudentTrackingApplicationBackEnd.Services
{
    public class AttendanceRepo : Repository<Attendance>, IAttendanceRepo
    {
        public AttendanceRepo(Context context) : base(context)
        {

        }
    }
}

