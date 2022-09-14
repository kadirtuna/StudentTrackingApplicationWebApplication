namespace StudentTrackingApplicationBackEnd.DTO
{
    public class ReadCourseDto
    {
        public int CourseId { get; set; } = 0;
        public string CourseName { get; set; } = string.Empty;
        public int CourseCredits { get; set; } = 0;
        public int CourseNumbers { get; set; } = 0;
        public int TeacherId { get; set; } = 0;
        public int CourseTotalPrice { get; set; } = 0;
        public DateTime CourseCreatedDate { get; set; } = DateTime.MinValue;
        public DateTime CourseStartDate { get; set; } = DateTime.MinValue;
        public int CourseDays { get; set; } = 0;
    }
}
