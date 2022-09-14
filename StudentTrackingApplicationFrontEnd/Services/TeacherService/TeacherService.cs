using StudentTrackingApplicationReal.Shared.Models;
using System.Net.Http.Json;

namespace StudentTrackingApplicationFrontEnd.Services.TeacherService
{
    public class TeacherService : ITeacherService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;

        public TeacherService(HttpClient http, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _authStateProvider = authStateProvider;
        }
        public List<Teacher> Teachers { get; set; } = new List<Teacher>();
        public int responsedStatusCodeFirstDigit { get; set; } = 0;

        public async Task<List<Teacher>> GetTeachers()
        {
            Teachers = new List<Teacher>();
            _authStateProvider.GetAuthenticationStateAsync();
            var result = await _http.GetFromJsonAsync<List<Teacher>>("api/Teacher");
            var response = await _http.GetAsync("api/teacher");
            int statusCode = Convert.ToInt16(response.StatusCode);
            int firstDigitOfStatusCode = statusCode;

            while (firstDigitOfStatusCode >= 10)
                firstDigitOfStatusCode /= 10;

            responsedStatusCodeFirstDigit = firstDigitOfStatusCode;

            if (result != null && firstDigitOfStatusCode == 2)
                Teachers = result;

            return Teachers;
        }
        public async Task<Teacher> GetTeacherById(int teacherId)
        {
            Teacher result = new Teacher();
            result = await _http.GetFromJsonAsync<Teacher>($"/api/teacher/{teacherId}");
            if (result != null)
                return result;
            throw new Exception("Teacher corresponding with this given teacherId not found!");
        }

        public async Task Add(CreateTeacherDto teacher)
        {
            await _http.PostAsJsonAsync<CreateTeacherDto>("api/teacher", teacher);
        }

        public async Task Update(int teacherId, CreateTeacherDto teacher)
        {
            await _http.PutAsJsonAsync($"api/teacher/{teacherId}", teacher);
        }

        public async Task Remove(int teacherId)
        {
            await _http.DeleteAsync($"api/teacher/{teacherId}");
        }

    }
}
