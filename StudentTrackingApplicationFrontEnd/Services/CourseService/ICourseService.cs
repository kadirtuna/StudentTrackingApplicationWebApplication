global using StudentTrackingApplicationReal.Shared.Models;

namespace StudentTrackingApplicationFrontEnd.Services.CourseService
{
    public interface ICourseService
    {
        public List<ReadCourseDto> Courses { get; set; }
        public int responsedStatusCodeFirstDigit { get; set; }
        public Task<List<ReadCourseDto>> GetCourses();
        public Task<ReadCourseDto> GetCourseById(int courseId);
        public Task Add(CreateCourseDto Course);
        public Task Update(int courseId, Course Course);
        public Task Remove(int courseId);
    }
}
