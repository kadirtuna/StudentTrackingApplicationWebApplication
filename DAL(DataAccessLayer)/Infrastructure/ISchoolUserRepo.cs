using StudentTrackingApplicationReal.Shared.Models;

namespace StudentTrackingApplicationBackEnd.Infrastructure
{
    public interface ISchoolUserRepo : IRepository<SchoolUser>
    {
        Task<SchoolUser> Get(string Id);
        void Remove(string userName);
    }
}
