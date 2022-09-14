namespace StudentTrackingApplicationBackEnd.Infrastructure
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Get(int Id);
        Task Add(TEntity entity);
        Task Remove(int Id);
        void Update(TEntity entity);
    }
}
