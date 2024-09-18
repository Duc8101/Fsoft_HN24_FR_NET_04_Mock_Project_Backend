using Microsoft.EntityFrameworkCore;
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

        public IQueryable<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include, Func<IQueryable<TEntity>, IQueryable<TEntity>>? sort, params Expression<Func<TEntity, bool>>[] predicates)
        {
            IQueryable<TEntity> query = GetQuery(include, predicates);
            if (sort != null)
            {
                query = sort(query);
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

        public async Task<TEntity?> GetSingleAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include, params Expression<Func<TEntity, bool>>[] predicates)
        {
            IQueryable<TEntity> query = GetQuery(include, predicates);
            return await query.SingleOrDefaultAsync();
        }

        public async Task<TEntity?> GetFirstAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include, params Expression<Func<TEntity, bool>>[] predicates)
        {
            IQueryable<TEntity> query = GetQuery(include, predicates);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<TEntity?> FindByIdAsync(object id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddMultipleAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        public void DeleteMultiple(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
            _context.SaveChanges();
        }

        public int Count(params Expression<Func<TEntity, bool>>[] predicates)
        {
            IQueryable<TEntity> query = GetQuery(null, predicates);
            return query.Count();
        }

        public async Task<int> CountAsync(params Expression<Func<TEntity, bool>>[] predicates)
        {
            IQueryable<TEntity> query = GetQuery(null, predicates);
            return await query.CountAsync();
        }

    }
}
