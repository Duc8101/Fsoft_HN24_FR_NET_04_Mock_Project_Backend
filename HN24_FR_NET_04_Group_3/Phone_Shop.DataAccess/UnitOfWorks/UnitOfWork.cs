using Phone_Shop.DataAccess.DBContext;

namespace Phone_Shop.DataAccess.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PhoneShopContext _context;

        public UnitOfWork(PhoneShopContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            _context.Database.CommitTransaction();
        }

        public async Task CommitAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void RollBack()
        {
            _context.Database.RollbackTransaction();
        }

        public async Task RollBackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}
