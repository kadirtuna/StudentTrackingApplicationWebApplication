using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StudentTrackingApplicationReal.Shared.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [StringLength(30)]
        public string StudentName { get; set; }
        [StringLength(25)]
        public string StudentSurname { get; set; }
        [StringLength(30)]
        public string StudentUserName { get; set; }
        [StringLength(50)]
        public string StudentEmail { get; set; }
        [StringLength(10)]
        public string StudentPhone { get; set; }
        public int StudentTotalCourseCredits { get; set; }  
        [Range(0, 100)]
        public int StudentGeneralAverageMark { get; set; }
        public int StudentDebts { get; set; }
        public DateTime StudentCreatedDate { get; set; }
    }

}
