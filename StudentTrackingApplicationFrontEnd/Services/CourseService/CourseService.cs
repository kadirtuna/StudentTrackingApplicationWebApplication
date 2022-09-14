using StudentTrackingApplicationReal.Shared.Models;
using System.Net.Http.Json;

namespace StudentTrackingApplicationFrontEnd.Services.CourseService
{
    public class CourseService : ICourseService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;

        public CourseService(HttpClient http, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _authStateProvider = authStateProvider;
        }
        public List<ReadCourseDto> Courses { get; set; } = new List<ReadCourseDto>();
        public int responsedStatusCodeFirstDigit { get; set; } = 0;

        public async Task<List<ReadCourseDto>> GetCourses()
        {
            Courses = new List<ReadCourseDto>();
            _authStateProvider.GetAuthenticationStateAsync();
            var result = await _http.GetFromJsonAsync<List<ReadCourseDto>>("api/Course");
            var response = await _http.GetAsync("api/Course");
            int statusCode = Convert.ToInt16(response.StatusCode);
            int firstDigitOfStatusCode = statusCode;

            while (firstDigitOfStatusCode >= 10)
                firstDigitOfStatusCode /= 10;

            responsedStatusCodeFirstDigit = firstDigitOfStatusCode;

            if (result != null && firstDigitOfStatusCode == 2)
                Courses = result;

            return Courses;
        }
        public async Task<ReadCourseDto> GetCourseById(int courseId)
        {
            ReadCourseDto result = new ReadCourseDto();
            result = await _http.GetFromJsonAsync<ReadCourseDto>($"/api/Course/{courseId}");
            if (result != null)
                return result;
            throw new Exception("Course corresponding with this given courseId not found!");
        }

        public async Task Add(CreateCourseDto Course)
        {
            await _http.PostAsJsonAsync<CreateCourseDto>("api/Course", Course);
        }

        public async Task Update(int courseId, Course Course)
        {
            await _http.PutAsJsonAsync($"api/Course/{courseId}", Course);
        }

        public async Task Remove(int courseId)
        {
            await _http.DeleteAsync($"api/Course/{courseId}");
        }

    }
}
