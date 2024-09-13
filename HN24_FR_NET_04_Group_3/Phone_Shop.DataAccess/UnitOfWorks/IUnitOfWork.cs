namespace Phone_Shop.DataAccess.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void BeginTransaction();
        void RollBack();
        Task CommitAsync();
        Task BeginTransactionAsync();
        Task RollBackAsync();
    }
}
