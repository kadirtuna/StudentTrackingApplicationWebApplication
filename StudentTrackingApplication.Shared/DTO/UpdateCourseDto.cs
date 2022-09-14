using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTrackingApplication.Shared.DTO
{
    public class UpdateCourseDto
    {
        public string CourseName { get; set; } = null;
        public int CourseCredits { get; set; } = 0;
        public int CourseNumbers { get; set; } = 0;
        public int TeacherId { get; set; } = 0;
        public int CourseTotalPrice { get; set; } = 0;
        public int CourseDays { get; set; } = 0;//The guideline about CourseDays is in the comment block below.
        public int TimeFirst2Digits { get; set; } = 0;
        public int TimeLast2Digits { get; set; } = 0;
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

