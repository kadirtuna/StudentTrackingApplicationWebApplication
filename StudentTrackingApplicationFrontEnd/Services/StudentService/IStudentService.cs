global using StudentTrackingApplicationReal.Shared.Models;

namespace StudentTrackingApplicationFrontEnd.Services.StudentService
{
    public interface IStudentService
    {
        public List<ReadStudentRestrictedDto> StudentsRestricted { get; set; }
        public List<ReadStudentDto> StudentsDetailed { get; set; }
        public int responsedStatusCodeFirstDigit { get; set; }
        public Task<List<ReadStudentRestrictedDto>> GetStudentsRestricted();
        public Task<List<ReadStudentDto>> GetStudentsDetailed();
        public Task<ReadStudentRestrictedDto> GetStudentRestrictedById(int studentId);
        public Task<ReadStudentDto> GetStudentDetailedById(int studentId);
        public Task Add(CreateStudentDto student);
        public Task Update(int studentId, UpdateStudentDto student);
        public Task Remove(int studentId);
    }
}
