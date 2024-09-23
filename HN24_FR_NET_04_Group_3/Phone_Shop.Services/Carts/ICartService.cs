using Phone_Shop.Common.DTOs.CartDTO;
using Phone_Shop.Common.Responses;

namespace Phone_Shop.Services.Carts
{
    public interface ICartService
    {
        ResponseBase List(int userId, string username);
        ResponseBase Create(CartCreateDTO DTO, int userId);
        ResponseBase Delete(int productId, int userId);
        ResponseBase Update(CartUpdateDTO DTO, int userId);
    }
}
