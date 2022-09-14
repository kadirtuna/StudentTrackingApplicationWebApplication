namespace StudentTrackingApplicationBackEnd.DTO
{
    public class UpdateStudentDto
    {
        public string StudentName { get; set; }
        public string StudentSurname { get; set; }
        public string StudentEmail { get; set; } = string.Empty;
        public string StudentPhone { get; set; } = string.Empty;
        public int StudentDebts { get; set; }
    }
}
