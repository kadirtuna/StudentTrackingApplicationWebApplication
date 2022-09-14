using System.ComponentModel.DataAnnotations;

namespace StudentTrackingApplicationReal.Shared.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }
        [StringLength(30)]
        public string TeacherName { get; set; }
        [StringLength(25)]
        public string TeacherSurname { get; set; }
        [StringLength(30)]
        public string TeacherUserName { get; set; }
        [StringLength(50)]
        public string TeacherEmail { get; set; }
        [StringLength(10)]
        public string TeacherPhone { get; set; }
        public DateTime TeacherCreatedDate { get; set; }
    }
}
