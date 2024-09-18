using Phone_Shop.Common.DTOs.OrderDTO;
using Phone_Shop.Common.Responses;

namespace Phone_Shop.Services.Orders
{
    public interface IOrderService
    {
        Task<ResponseBase> Create(OrderCreateDTO DTO, int userId);
        ResponseBase List(string? status, int pageSize, int currentPage, int? userId);
        ResponseBase Detail(int orderId, int? userId);
        Task<ResponseBase> Update(int orderId, OrderUpdateDTO DTO);
    }
}
