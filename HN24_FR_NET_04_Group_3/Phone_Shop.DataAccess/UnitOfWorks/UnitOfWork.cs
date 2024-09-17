using Phone_Shop.Common.Entity;
using Phone_Shop.DataAccess.DBContext;
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

    public UnitOfWork(PhoneShopContext context, IRepository<User> userRepository, IRepository<Client> clientRepository
        , IRepository<UserClient> userClientRepository, IRepository<Cart> cartRepository, IRepository<Product> productRepository
        , IRepository<Order> orderRepository, IRepository<OrderDetail> orderDetailRepository, IRepository<Category> categoryRepository)
    {
      _context = context;
      _userRepository = userRepository;
      _clientRepository = clientRepository;
      _userClientRepository = userClientRepository;
      _cartRepository = cartRepository;
      _productRepository = productRepository;
      _orderRepository = orderRepository;
      _orderDetailRepository = orderDetailRepository;
      _categoryRepository = categoryRepository;
    }

    public IRepository<User> UserRepository => _userRepository;

    public IRepository<Client> ClientRepository => _clientRepository;

    public IRepository<UserClient> UserClientRepository => _userClientRepository;

    public IRepository<Cart> CartRepository => _cartRepository;

    public IRepository<Product> ProductRepository => _productRepository;

    public IRepository<Order> OrderRepository => _orderRepository;

    public IRepository<OrderDetail> OrderDetailRepository => _orderDetailRepository;

    public IRepository<Category> CategoryRepository => _categoryRepository;

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
