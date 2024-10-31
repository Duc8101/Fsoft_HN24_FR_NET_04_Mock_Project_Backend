using Microsoft.EntityFrameworkCore.Storage;
using Phone_Shop.DataAccess.DBContext;
using Phone_Shop.DataAccess.Entity;
using Phone_Shop.DataAccess.Repositories.Common;

namespace Phone_Shop.DataAccess.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly PhoneShopContext _context;
        private readonly IRepository<User>? _userRepository;
        private readonly IRepository<UserToken>? _userTokenRepository;
        private readonly IRepository<Cart>? _cartRepository;
        private readonly IRepository<Product>? _productRepository;
        private readonly IRepository<Order>? _orderRepository;
        private readonly IRepository<OrderDetail>? _orderDetailRepository;
        private readonly IRepository<Category>? _categoryRepository;
        private readonly IRepository<Feedback>? _feedbackRepository;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(PhoneShopContext context)
        {
            _context = context;
        }

        public UnitOfWork(PhoneShopContext context, IRepository<User> userRepository, IRepository<UserToken> userTokenRepository
            , IRepository<Cart> cartRepository, IRepository<Product> productRepository, IRepository<Order> orderRepository
            , IRepository<OrderDetail> orderDetailRepository, IRepository<Category> categoryRepository, IRepository<Feedback> feedbackRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _categoryRepository = categoryRepository;
            _feedbackRepository = feedbackRepository;
        }

        public IRepository<User> UserRepository => _userRepository ?? new Repository<User>(_context);

        public IRepository<UserToken> UserTokenRepository => _userTokenRepository ?? new Repository<UserToken>(_context);

        public IRepository<Cart> CartRepository => _cartRepository ?? new Repository<Cart>(_context);

        public IRepository<Product> ProductRepository => _productRepository ?? new Repository<Product>(_context);

        public IRepository<Order> OrderRepository => _orderRepository ?? new Repository<Order>(_context);

        public IRepository<OrderDetail> OrderDetailRepository => _orderDetailRepository ?? new Repository<OrderDetail>(_context);

        public IRepository<Category> CategoryRepository => _categoryRepository ?? new Repository<Category>(_context);

        public IRepository<Feedback> FeedbackRepository => _feedbackRepository ?? new Repository<Feedback>(_context);

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
            }
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            if (_transaction != null)
            {
                _transaction.Dispose();
            }
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
    }
}
