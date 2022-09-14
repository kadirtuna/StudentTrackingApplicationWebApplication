using StudentTrackingApplicationReal.Shared.Models;
namespace StudentTrackingApplicationBackEnd.DTO
{
    public class CreateStudentDto
    {
        //public int StudentId { get; set; } = 0;
        public string StudentName { get; set; } = string.Empty;
        public string StudentSurname { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public string StudentPhone { get; set; } = string.Empty;
    }
}
