using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Common.DTOs.CartDTO;
using Phone_Shop.Common.Entity;
using Phone_Shop.Common.Responses;
using Phone_Shop.DataAccess.UnitOfWorks;
using Phone_Shop.Services.Base;
using System.Linq.Expressions;
using System.Net;

namespace Phone_Shop.Services.Carts
{
    public class CartService : BaseService, ICartService
    {

        public CartService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public ResponseBase Create(CartCreateDTO DTO, int userId)
        {
            try
            {
                Product? product = _unitOfWork.ProductRepository.GetSingle(null, p => p.ProductId == DTO.ProductId && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseBase($"Not found product with id = {DTO.ProductId}", (int)HttpStatusCode.NotFound);
                }

                Expression<Func<Cart, bool>> predicate = c => c.CustomerId == userId && c.ProductId == DTO.ProductId && c.IsCheckout == false && c.IsDeleted == false;
                Cart? cart = _unitOfWork.CartRepository.GetFirst(null, predicate);

                _unitOfWork.BeginTransaction();
                if (cart == null)
                {
                    cart = new Cart()
                    {
                        CustomerId = userId,
                        ProductId = DTO.ProductId,
                        Quantity = 1,
                        IsCheckout = false,
                        CreatedAt = DateTime.Now,
                        UpdateAt = DateTime.Now,
                        IsDeleted = false,
                    };
                    _unitOfWork.CartRepository.Add(cart);
                }
                else
                {
                    cart.Quantity++;
                    cart.UpdateAt = DateTime.Now;
                    _unitOfWork.CartRepository.Update(cart);
                }

                _unitOfWork.Commit();
                return new ResponseBase(true);
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Delete(int productId, int userId)
        {
            try
            {
                Product? product = _unitOfWork.ProductRepository.GetSingle(null, p => p.ProductId == productId && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseBase($"Not found product with id = {productId}", (int)HttpStatusCode.NotFound);
                }

                Expression<Func<Cart, bool>> predicate = c => c.CustomerId == userId && c.ProductId == productId && c.IsCheckout == false && c.IsDeleted == false;
                Cart? cart = _unitOfWork.CartRepository.GetFirst(null, predicate);
                if (cart == null)
                {
                    return new ResponseBase("Not found cart", (int)HttpStatusCode.NotFound);
                }

                if (cart.Quantity == 1)
                {
                    cart.IsDeleted = true;
                }
                else
                {
                    cart.Quantity--;
                    cart.UpdateAt = DateTime.Now;
                }

                _unitOfWork.BeginTransaction();
                _unitOfWork.CartRepository.Update(cart);
                _unitOfWork.Commit();
                return new ResponseBase(true);
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase List(int userId, string username)
        {
            try
            {
                Func<IQueryable<Cart>, IQueryable<Cart>> include = item => item.Include(c => c.Product);
                Expression<Func<Cart, bool>> predicate = c => c.CustomerId == userId && c.IsCheckout == false && c.IsDeleted == false;
                IQueryable<Cart> query = _unitOfWork.CartRepository.GetAll(include, null, predicate);
                List<Cart> carts = query.ToList();
                List<CartDetailDTO> cartDetailDTOs = _mapper.Map<List<CartDetailDTO>>(carts);
                CartListDTO data = new CartListDTO()
                {
                    CartDetailDTOs = cartDetailDTOs,
                    Customer = username
                };

                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
