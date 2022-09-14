namespace StudentTrackingApplicationBackEnd.DTO
{
    public class ReadStudentCourseDto
    {
        public int StudentCourseId { get; set; } = 0;
        public int StudentId { get; set; } = 0;
        public int StudentMidTermMark { get; set; } = 0;
        public int StudentFinalMark { get; set; } = 0;
        public int StudentAverageMark { get; set; } = 0;
        public int CourseId { get; set; } = 0;
        public int TeacherId { get; set; } = 0;
    }
}
