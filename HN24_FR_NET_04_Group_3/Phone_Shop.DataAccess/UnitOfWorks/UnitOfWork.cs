using Phone_Shop.DataAccess.DBContext;
using Phone_Shop.DataAccess.Entity;
using Phone_Shop.DataAccess.Repositories.Common;

namespace Phone_Shop.DataAccess.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly PhoneShopContext _context;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly IRepository<UserClient> _userClientRepository;
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Feedback> _feedbackRepository;

        public UnitOfWork(PhoneShopContext context)
        {
            _context = context;
        }

        public IRepository<User> UserRepository => _userRepository ?? new Repository<User>(_context);

        public IRepository<Client> ClientRepository => _clientRepository ?? new Repository<Client>(_context);

        public IRepository<UserClient> UserClientRepository => _userClientRepository ?? new Repository<UserClient>(_context);

        public IRepository<Cart> CartRepository => _cartRepository ?? new Repository<Cart>(_context);

        public IRepository<Product> ProductRepository => _productRepository ?? new Repository<Product>(_context);

        public IRepository<Order> OrderRepository => _orderRepository ?? new Repository<Order>(_context);

        public IRepository<OrderDetail> OrderDetailRepository => _orderDetailRepository ?? new Repository<OrderDetail>(_context);

        public IRepository<Category> CategoryRepository => _categoryRepository ?? new Repository<Category>(_context);

        public IRepository<Feedback> FeedbackRepository => _feedbackRepository ?? new Repository<Feedback>(_context);

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
