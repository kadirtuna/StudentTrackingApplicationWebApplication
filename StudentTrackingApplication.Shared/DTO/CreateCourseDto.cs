namespace StudentTrackingApplicationBackEnd.DTO
{
    public class CreateCourseDto
    {
        public string CourseName { get; set; } = null;
        public int CourseCredits { get; set; } = 0;
        public int CourseNumbers { get; set; } = 0;
        public int TeacherId { get; set; } = 0;
        public int CourseTotalPrice { get; set; } = 0;
        public int CourseDays { get; set; } = 0;//The guideline about CourseDays is in the comment block below.
        public int TimeFirst2Digits { get; set; } = 0;
        public int TimeLast2Digits { get; set; } = 0;
        public static List<int> DayAdjuster(int CourseDays)
        {
            List<int> dayAdjuster = new List<int>();

            switch (CourseDays)
            {
                case 1:
                    dayAdjuster.Add(1);
                    break;
                case 2:
                    dayAdjuster.Add(2);
                    break;
                case 3:
                    dayAdjuster.Add(3);
                    break;
                case 4:
                    dayAdjuster.Add(4);
                    break;
                case 5:
                    dayAdjuster.Add(5);
                    break;
                case 6:
                    dayAdjuster.Add(1);
                    dayAdjuster.Add(2);
                    break;
                case 7:
                    dayAdjuster.Add(1);
                    dayAdjuster.Add(3);
                    break;
                case 8:
                    dayAdjuster.Add(1);
                    dayAdjuster.Add(4);
                    break;
                case 9:
                    dayAdjuster.Add(1);
                    dayAdjuster.Add(5);
                    break;
                case 10:
                    dayAdjuster.Add(2);
                    dayAdjuster.Add(3);
                    break;
                case 11:
                    dayAdjuster.Add(2);
                    dayAdjuster.Add(4);
                    break;
                case 12:
                    dayAdjuster.Add(2);
                    dayAdjuster.Add(5);
                    break;
                case 13:
                    dayAdjuster.Add(3);
                    dayAdjuster.Add(4);
                    break;
                case 14:
                    dayAdjuster.Add(3);
                    dayAdjuster.Add(5);
                    break;
                case 15:
                    dayAdjuster.Add(4);
                    dayAdjuster.Add(5);
                    break;
                default:
                    dayAdjuster.Add(0);
                    break;
            }
            return dayAdjuster;
        }
        enum WeekDays
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday,
        }
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

