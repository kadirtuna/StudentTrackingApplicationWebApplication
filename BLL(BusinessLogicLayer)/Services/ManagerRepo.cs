using StudentTrackingApplicationBackEnd.Infrastructure;
using StudentTrackingApplicationReal.Shared.Models;

namespace StudentTrackingApplicationBackEnd.Services
{
    public class ManagerRepo : Repository<Manager>, IManagerRepo
    {
        public ManagerRepo(Context _dbContext) : base(_dbContext)
        {
        }
    }
}
