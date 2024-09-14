using System.Linq.Expressions;

namespace Phone_Shop.DataAccess.Repositories.Queries
{
    public interface IQueryRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include, Func<IQueryable<TEntity>, IQueryable<TEntity>>? sort, params Expression<Func<TEntity, bool>>[] predicates);

        TEntity? GetSingle(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include, params Expression<Func<TEntity, bool>>[] predicates);

        TEntity? GetFirst(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include, params Expression<Func<TEntity, bool>>[] predicates);

        IQueryable<TEntity> GetAll(params Expression<Func<TEntity, bool>>[] predicates);

        TEntity? GetSingle(params Expression<Func<TEntity, bool>>[] predicates);

        TEntity? GetFirst(params Expression<Func<TEntity, bool>>[] predicates);

        TEntity? FindById(object id);

        Task<TEntity?> GetSingleAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include, params Expression<Func<TEntity, bool>>[] predicates);

        Task<TEntity?> GetFirstAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include, params Expression<Func<TEntity, bool>>[] predicates);

        Task<TEntity?> GetSingleAsync(params Expression<Func<TEntity, bool>>[] predicates);

        Task<TEntity?> GetFirstAsync(params Expression<Func<TEntity, bool>>[] predicates);

        Task<TEntity?> FindByIdAsync(object id);
    }
}
