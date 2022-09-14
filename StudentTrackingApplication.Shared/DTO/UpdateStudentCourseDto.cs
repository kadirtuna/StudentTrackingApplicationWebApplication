namespace StudentTrackingApplicationBackEnd.DTO
{
    public class UpdateStudentCourseDto
    {
        public int StudentId { get; set; } = 0;
        public int CourseId { get; set; } = 0;
        public int StudentMidTermMark { get; set; } = 0;
        public int StudentFinalMark { get; set; } = 0;
    }
}
