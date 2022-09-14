using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StudentTrackingApplicationReal.Shared.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public int AttendancePrice { get; set; }
        public bool AttendanceState { get; set; }
        public bool AttendancePaymentState { get; set; }
        [Required]
        [ForeignKey("StudentCourseId")]
        public StudentCourse StudentCourse { get; set; }
        public int StudentCourseId { get; set; }


    }
}
