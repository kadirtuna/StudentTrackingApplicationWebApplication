namespace StudentTrackingApplicationBackEnd.DTO
{
    public class CreateTeacherDto
    {
        public string TeacherName { get; set; } = string.Empty;
        public string TeacherSurname { get; set; } = string.Empty;
        public string TeacherEmail { get; set; } = string.Empty;
        public string TeacherPhone { get; set; } = string.Empty;
    }
}
