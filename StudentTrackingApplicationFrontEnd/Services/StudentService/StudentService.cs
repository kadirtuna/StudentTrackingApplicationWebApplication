using StudentTrackingApplicationReal.Shared.Models;
using System.Net.Http.Json;

namespace StudentTrackingApplicationFrontEnd.Services.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;

        public StudentService(HttpClient http, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _authStateProvider = authStateProvider;
        }
        public List<ReadStudentRestrictedDto> StudentsRestricted { get; set; } = new List<ReadStudentRestrictedDto>();
        public List<ReadStudentDto> StudentsDetailed { get; set; } = new List<ReadStudentDto>();
        public int responsedStatusCodeFirstDigit { get; set; } = 0;

        public async Task<List<ReadStudentRestrictedDto>> GetStudentsRestricted()
        {
            StudentsRestricted = new List<ReadStudentRestrictedDto>();
            _authStateProvider.GetAuthenticationStateAsync();
            var result = await _http.GetFromJsonAsync<List<ReadStudentRestrictedDto>>("api/student/GeneralStudentsInformation");
            var response = await _http.GetAsync("api/student/GeneralStudentsInformation");
            int statusCode = Convert.ToInt16(response.StatusCode);
            int firstDigitOfStatusCode = statusCode;

            while (firstDigitOfStatusCode >= 10)
                firstDigitOfStatusCode /= 10;

            responsedStatusCodeFirstDigit = firstDigitOfStatusCode;

            if (result != null && firstDigitOfStatusCode == 2)
                StudentsRestricted = result;

            return StudentsRestricted;
        }

        public async Task<List<ReadStudentDto>> GetStudentsDetailed()
        {
            StudentsDetailed = new List<ReadStudentDto>();
            _authStateProvider.GetAuthenticationStateAsync();
            var result = await _http.GetFromJsonAsync<List<ReadStudentDto>>("api/student/DetailedStudentsInformation");
            var response = await _http.GetAsync("api/student/GeneralStudentsInformation");
            int statusCode = Convert.ToInt16(response.StatusCode);
            int firstDigitOfStatusCode = statusCode;

            while (firstDigitOfStatusCode >= 10)
                firstDigitOfStatusCode /= 10;

            responsedStatusCodeFirstDigit = firstDigitOfStatusCode;

            if (result != null && firstDigitOfStatusCode == 2)
                StudentsDetailed = result;

            return StudentsDetailed;
        }


        public async Task<ReadStudentRestrictedDto> GetStudentRestrictedById(int studentId)
        {
            ReadStudentRestrictedDto result = new ReadStudentRestrictedDto();
            result = await _http.GetFromJsonAsync<ReadStudentRestrictedDto>($"/api/Student/{studentId}/GeneralStudentInformation");
            if (result != null)
                return result;
            throw new Exception("Student corresponding with this given studentId not found!");
        }

        public async Task<ReadStudentDto> GetStudentDetailedById(int studentId)
        {
            ReadStudentDto result = new ReadStudentDto();
            result = await _http.GetFromJsonAsync<ReadStudentDto>($"/api/Student/{studentId}/DetailedStudentInformation");
            if (result != null)
                return result;
            throw new Exception("Student corresponding with this given studentId not found!");
        }

        public async Task Add(CreateStudentDto student)
        {
            await _http.PostAsJsonAsync<CreateStudentDto>("api/Student", student);
        }

        public async Task Update(int studentId, UpdateStudentDto student)
        {
            await _http.PutAsJsonAsync($"api/Student/{studentId}", student);
        }

        public async Task Remove(int studentId)
        {
            await _http.DeleteAsync($"api/student/{studentId}");
        }
    }
}
