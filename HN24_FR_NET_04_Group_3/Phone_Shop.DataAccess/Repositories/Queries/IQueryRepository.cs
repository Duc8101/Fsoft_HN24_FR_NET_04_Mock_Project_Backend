using System.Linq.Expressions;

namespace Phone_Shop.DataAccess.Repositories.Queries
{
    public interface IQueryRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include, params Expression<Func<TEntity, bool>>[] predicates);
        
        TEntity? GetSingle(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include, params Expression<Func<TEntity, bool>>[] predicates);
        
        TEntity? GetFirst(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include, params Expression<Func<TEntity, bool>>[] predicates);
        
        TEntity? FindById(object id);       
    }
}
