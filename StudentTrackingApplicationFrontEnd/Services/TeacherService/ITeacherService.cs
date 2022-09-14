global using StudentTrackingApplicationReal.Shared.Models;

namespace StudentTrackingApplicationFrontEnd.Services.TeacherService
{
    public interface ITeacherService
    {
        public List<Teacher> Teachers { get; set; }
        public int responsedStatusCodeFirstDigit { get; set; }
        public Task<List<Teacher>> GetTeachers();
        public Task<Teacher> GetTeacherById(int teacherId);
        public Task Add(CreateTeacherDto teacher);
        public Task Update(int teacherId, CreateTeacherDto teacher);
        public Task Remove(int teacherId);
    }
}
