using StudentTrackingApplicationReal.Shared.Models;

namespace StudentTrackingApplicationBackEnd.DTO
{
    public class ReadAttendanceDto
    {
        public int AttendanceId { get; set; } = 0;
        public DateTime AttendanceDate { get; set; } = DateTime.MinValue;
        public int AttendancePrice { get; set; } = 0;
        public bool AttendanceState { get; set; } = false;
        public bool AttendancePaymentState { get; set; } = false;
        public int StudentCourseId { get; set; } = 0;
        public int StudentId { get; set; } = 0;
        public int CourseId { get; set; } = 0;
        public int TeacherId { get; set; } = 0;
    }
}
