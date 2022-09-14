namespace StudentTrackingApplicationBackEnd.DTO
{
    public class ReadStudentDto
    {
        public int StudentId { get; set; } = 0;
        public string StudentName { get; set; } = string.Empty;
        public string StudentSurname { get; set; } = string.Empty;
        public string StudentUserName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public string StudentPhone { get; set; } = string.Empty;
        public int StudentTotalCourseCredits { get; set; } = 0;
        public int StudentGeneralAverageMark { get; set; } = 0;
        public int StudentDebts { get; set; } = 0;
    }
}
