using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Common.DTOs.CartDTO;
using Phone_Shop.Common.DTOs.OrderDTO;
using Phone_Shop.Common.Entity;
using Phone_Shop.Common.Enums;
using Phone_Shop.Common.Responses;
using Phone_Shop.DataAccess.Helper;
using Phone_Shop.DataAccess.UnitOfWorks;
using Phone_Shop.Services.Base;
using System.Linq.Expressions;
using System.Net;

namespace Phone_Shop.Services.Orders
{
    public class OrderService : BaseService, IOrderService
    {
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ResponseBase> Create(OrderCreateDTO DTO, int userId)
        {
            try
            {
                Func<IQueryable<Cart>, IQueryable<Cart>> include = item => item.Include(c => c.Product).Include(c => c.Customer);
                Expression<Func<Cart, bool>> predicateCart = c => c.CustomerId == userId;
                IQueryable<Cart> queryCart = _unitOfWork.CartRepository.GetAll(include, null, predicateCart);
                List<Cart> carts = queryCart.ToList();

                if (carts.Count == 0)
                {
                    return new ResponseBase("You can't checkout when your cart is empty", (int)HttpStatusCode.Conflict);
                }

                List<CartDetailDTO> cartDetailDTOs = _mapper.Map<List<CartDetailDTO>>(carts);
                CartListDTO data = new CartListDTO()
                {
                    CartDetailDTOs = cartDetailDTOs,
                    Customer = carts[0].Customer.Username,
                };

                if (StringHelper.isStringNullOrEmpty(DTO.Address))
                {
                    return new ResponseBase(data, "You have to input address", (int)HttpStatusCode.Conflict);
                }

                foreach (CartDetailDTO item in data.CartDetailDTOs)
                {
                    Product? product = _unitOfWork.ProductRepository.GetSingle(null, p => p.ProductId == item.ProductId && p.IsDeleted == false);
                    if (product == null)
                    {
                        return new ResponseBase(data, $"Product '{item.ProductName}' not exist!!!", (int)HttpStatusCode.NotFound);
                    }

                    if (product.Quantity < item.Quantity)
                    {
                        return new ResponseBase(data, $"Product '{item.ProductName}' not have enough quantity!!!", (int)HttpStatusCode.Conflict);
                    }
                }

                string body = UserHelper.BodyEmailForAdminReceiveOrder();
                Expression<Func<User, bool>> predicateUser = u => u.RoleId == (int)Roles.Admin;
                IQueryable<User> queryUser = _unitOfWork.UserRepository.GetAll(null, null, predicateUser);
                List<string> emails = queryUser.Select(u => u.Email).ToList();
                if (emails.Count > 0)
                {
                    foreach (string email in emails)
                    {
                        await UserHelper.sendEmail("[PHONE SHOP] Notification for new order", body, email);
                    }
                }

                _unitOfWork.BeginTransaction();
                Order order = _mapper.Map<Order>(DTO);
                order.Status = OrderStatus.Pending.ToString();
                order.CreatedAt = DateTime.Now;
                order.UpdateAt = DateTime.Now;
                order.IsDeleted = false;
                order.Note = null;
                order.CustomerId = userId;
                _unitOfWork.OrderRepository.Add(order);

                List<OrderDetail> orderDetails = _mapper.Map<List<OrderDetail>>(data.CartDetailDTOs);
                orderDetails.ForEach(detail =>
                {
                    detail.OrderId = order.OrderId;
                    detail.CreatedAt = DateTime.Now;
                    detail.UpdateAt = DateTime.Now;
                    detail.IsDeleted = false;
                });

                _unitOfWork.OrderDetailRepository.AddMultiple(orderDetails);
                _unitOfWork.CartRepository.DeleteMultiple(carts);
                _unitOfWork.Commit();
                return new ResponseBase(data, "Check out successful");
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
