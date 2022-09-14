using StudentTrackingApplicationReal.Shared.Models;
using System.Net.Http.Json;

namespace StudentTrackingApplicationFrontEnd.Services.ManagerService
{
    public class ManagerService : IManagerService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;

        public ManagerService(HttpClient http, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _authStateProvider = authStateProvider;
        }
        public List<Manager> Managers { get; set; } = new List<Manager>();
        public int responsedStatusCodeFirstDigit { get; set; } = 0;

        public async Task<List<Manager>> GetManagers()
        {
            Managers = new List<Manager>();
            _authStateProvider.GetAuthenticationStateAsync();
            var result = await _http.GetFromJsonAsync<List<Manager>>("api/manager");
            var response = await _http.GetAsync("api/manager");
            int statusCode = Convert.ToInt16(response.StatusCode);
            int firstDigitOfStatusCode = statusCode;

            while (firstDigitOfStatusCode >= 10)
                firstDigitOfStatusCode /= 10;

            responsedStatusCodeFirstDigit = firstDigitOfStatusCode;

            if (result != null && firstDigitOfStatusCode == 2)
                Managers = result;

            return Managers;
        }
        public async Task<Manager> GetManagerById(int managerId)
        {
            Manager result = new Manager();
            result = await _http.GetFromJsonAsync<Manager>($"/api/manager/{managerId}");
            if (result != null)
                return result;
            throw new Exception("manager corresponding with this given managerId not found!");
        }

        public async Task Add(CreateManagerDto manager)
        {
            await _http.PostAsJsonAsync<CreateManagerDto>("api/manager", manager);
        }

        public async Task Update(int managerId, CreateManagerDto manager)
        {
            await _http.PutAsJsonAsync($"api/manager/{managerId}", manager);
        }

        public async Task Remove(int managerId)
        {
            await _http.DeleteAsync($"api/manager/{managerId}");
        }

    }
}
