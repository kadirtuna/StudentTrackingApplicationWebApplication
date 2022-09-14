global using StudentTrackingApplicationReal.Shared.Models;

namespace StudentTrackingApplicationFrontEnd.Services.ManagerService
{
    public interface IManagerService
    {
        public List<Manager> Managers { get; set; }
        public int responsedStatusCodeFirstDigit { get; set; }
        public Task<List<Manager>> GetManagers();
        public Task<Manager> GetManagerById(int managerId);
        public Task Add(CreateManagerDto manager);
        public Task Update(int managerId, CreateManagerDto manager);
        public Task Remove(int managerId);
    }
}
