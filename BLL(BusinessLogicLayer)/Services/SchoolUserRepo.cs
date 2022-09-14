using StudentTrackingApplicationReal.Shared.Models;
using StudentTrackingApplicationBackEnd.Infrastructure;

namespace StudentTrackingApplicationBackEnd.Services
{
    public class SchoolUserRepo : Repository<SchoolUser>, Infrastructure.ISchoolUserRepo
    {
        protected readonly Context dbContext;

        public SchoolUserRepo(Context context) : base(context)
        {
            dbContext = context;
        }

        async Task<SchoolUser> ISchoolUserRepo.Get(string userName)
        {
            return await dbContext.SchoolUsers.FindAsync(userName);
        }

        async Task ISchoolUserRepo.Remove(string userName)
        {
            SchoolUser user = await dbContext.SchoolUsers.FindAsync(userName);
            dbContext.SchoolUsers.Remove(user);
        }
    }
}
