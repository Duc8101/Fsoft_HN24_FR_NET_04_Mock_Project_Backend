using Phone_Shop.DataAccess.DBContext;
using System.Linq.Expressions;

namespace Phone_Shop.DataAccess.Repositories.Common
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        private readonly PhoneShopContext _context;

        public Repository(PhoneShopContext context)
        {
            _context = context;
        }

        private IQueryable<TEntity> GetQuery(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include, params Expression<Func<TEntity, bool>>[] predicates)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (include != null)
            {
                query = include(query);
            }

            if (predicates != null && predicates.Length > 0)
            {
                foreach (Expression<Func<TEntity, bool>> predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }
            return query;
        }

        public TEntity? GetSingle(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include, params Expression<Func<TEntity, bool>>[] predicates)
        {
            IQueryable<TEntity> query = GetQuery(include, predicates);
            return query.SingleOrDefault();
        }

        public TEntity? GetFirst(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include, params Expression<Func<TEntity, bool>>[] predicates)
        {
            IQueryable<TEntity> query = GetQuery(include, predicates);
            return query.FirstOrDefault();
        }

        public IQueryable<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include, params Expression<Func<TEntity, bool>>[] predicates)
        {
            return GetQuery(include, predicates);
        }

        public TEntity? FindById(object id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            _context.SaveChanges();
        }

        public void AddMultiple(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
            _context.SaveChanges();
        }

        public void UpdateMultiple(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);
            _context.SaveChanges();
        }
    }
}
