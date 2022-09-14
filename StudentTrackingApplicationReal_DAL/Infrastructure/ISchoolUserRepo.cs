using StudentTrackingApplicationReal.Shared.Models;

namespace StudentTrackingApplicationBackEnd.Infrastructure
{
    public interface ISchoolUserRepo : IRepository<SchoolUser>
    {
        public Task<SchoolUser> Get(string Id);
        public Task Remove(string userName);
    }
}
