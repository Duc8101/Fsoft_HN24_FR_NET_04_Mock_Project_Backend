namespace Phone_Shop.DataAccess.Repositories.Commands
{
    public interface ICommandRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);

        void Update(TEntity entity);

        void AddMultiple(IEnumerable<TEntity> entities);

        void UpdateMultiple(IEnumerable<TEntity> entities);

        Task AddAsync(TEntity entity);

        Task AddMultipleAsync(IEnumerable<TEntity> entities);
    }
}
