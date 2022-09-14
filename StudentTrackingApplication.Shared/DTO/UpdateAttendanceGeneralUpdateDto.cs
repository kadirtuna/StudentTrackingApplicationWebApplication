namespace StudentTrackingApplicationBackEnd.DTO
{
    public class UpdateAttendanceGeneralUpdateDto
    {
        public DateTime AttendanceDate { get; set; } = DateTime.MinValue;
        public bool AttendanceState { get; set; } = false;
        public bool AttendancePaymentState { get; set; } = false;
    }
}
