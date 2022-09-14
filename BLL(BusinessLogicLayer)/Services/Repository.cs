using Microsoft.EntityFrameworkCore;
using StudentTrackingApplicationBackEnd.Infrastructure;
using StudentTrackingApplicationReal.Shared.Models;

namespace StudentTrackingApplicationBackEnd.Services
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly Context dbContext;
        public Repository(Context _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task Remove(int Id)
        {
            TEntity entity = dbContext.Set<TEntity>().Find(Id); 
            dbContext.Set<TEntity>().Remove(entity);
        }

        public async void Update(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
        }

        async Task IRepository<TEntity>.Add(TEntity entity)
        {
            await dbContext.Set<TEntity>().AddAsync(entity);
        }

        async Task<TEntity> IRepository<TEntity>.Get(int Id)
        {
            return await dbContext.Set<TEntity>().FindAsync(Id);
        }

        async Task<IEnumerable<TEntity>> IRepository<TEntity>.GetAll()
        {
            return await dbContext.Set<TEntity>().ToListAsync();
        }
    }
}
