using Phone_Shop.DataAccess.Repositories.Commands;
using Phone_Shop.DataAccess.Repositories.Queries;

namespace Phone_Shop.DataAccess.Repositories.Common
{
    public interface IRepository<TEntity> : IQueryRepository<TEntity>, ICommandRepository<TEntity> where TEntity : class
    {

    }
}
