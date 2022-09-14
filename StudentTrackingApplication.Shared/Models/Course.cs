using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StudentTrackingApplicationReal.Shared.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [StringLength(50)]
        public string CourseName { get; set; }
        public int CourseCredits { get; set; }
        public int CourseNumbers { get; set; }
        public int CourseTotalPrice { get; set; } 
        public DateTime CourseCreatedDate { get; set; }
        public DateTime CourseStartDate { get; set; }
        public int CourseDays { get; set; } = 0;//The guideline about CourseDays is in the comment block below.}
        [Required]
        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; }
        public int TeacherId { get; set; }
        public int TimeFirst2Digits { get; set; } 
        public int TimeLast2Digits { get; set; }

    }
}
/*The CourseDays Table is here below
1 - Monday,
2 - Tuesday,
3 - Wednesday,
4 - Thursday,
5 - Friday,
6 - Monday and Tuesday,
7 - Monday and Wednesday,
8 - Monday and Thursday,
9 - Monday and Friday,
10 - Tuesday and Wednesday,
11 - Tuesday and Thurday,
12 - Tuesday and Friday,
13 - Wednesday and Thursday,
14 - Wednesday and Friday,
15 - Thursday and Friday*/
