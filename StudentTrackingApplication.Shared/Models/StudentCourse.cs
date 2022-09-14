using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentTrackingApplicationReal.Shared.Models
{
    public class StudentCourse
    {
        [Key]
        public int StudentCourseId { get; set; }
        [Range(-1, 100)]
        public int StudentMidTermMark { get; set; }
        [Range(-1, 100)]
        public int StudentFinalMark { get; set; }
        [Range(-1, 100)]
        public int StudentAverageMark { get; set; }
        [Required]
        [ForeignKey("StudentId")]
        public Student Student { get; set; }
        public int StudentId { get; set; }
        [Required]
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        public int CourseId { get; set; }
    }
}
