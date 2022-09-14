namespace StudentTrackingApplicationBackEnd.DTO
{
    public class CreateSchoolUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
    }
}
