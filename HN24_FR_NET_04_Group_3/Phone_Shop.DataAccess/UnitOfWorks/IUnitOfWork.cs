using Phone_Shop.DataAccess.Entity;
using Phone_Shop.DataAccess.Repositories.Common;

namespace Phone_Shop.DataAccess.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }
        IRepository<Client> ClientRepository { get; }
        IRepository<UserClient> UserClientRepository { get; }
        IRepository<Cart> CartRepository { get; }
        IRepository<Product> ProductRepository { get; }
        IRepository<Order> OrderRepository { get; }
        IRepository<OrderDetail> OrderDetailRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IRepository<Feedback> FeedbackRepository { get; }

        void Commit();
        void BeginTransaction();
        void RollBack();
        Task CommitAsync();
        Task BeginTransactionAsync();
        Task RollBackAsync();
    }
}
