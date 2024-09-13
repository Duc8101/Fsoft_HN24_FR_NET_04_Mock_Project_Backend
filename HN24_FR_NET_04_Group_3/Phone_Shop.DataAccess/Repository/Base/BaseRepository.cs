using Phone_Shop.DataAccess.DBContext;

namespace Phone_Shop.DataAccess.Repository.Base
{
    public class BaseRepository
    {
        private protected readonly PhoneShopContext _context;

        public BaseRepository(PhoneShopContext context)
        {
            _context = context;
        }
    }
}
