namespace StudentTrackingApplicationBackEnd.DTO
{
    public class ReadCoursesPricesDto
    {
        public int CourseId { get; set; } = 0;
        public string CourseName { get; set; } = null;
        public int CourseTotalPrice { get; set; } = 0;

    }
}
