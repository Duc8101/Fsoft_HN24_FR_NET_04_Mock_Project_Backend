﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Common.DTOs.CartDTO;
using Phone_Shop.Common.DTOs.OrderDetailDTO;
using Phone_Shop.Common.DTOs.OrderDTO;
using Phone_Shop.Common.Entity;
using Phone_Shop.Common.Enums;
using Phone_Shop.Common.Paging;
using Phone_Shop.Common.Responses;
using Phone_Shop.DataAccess.Extensions;
using Phone_Shop.DataAccess.Helper;
using Phone_Shop.DataAccess.UnitOfWorks;
using Phone_Shop.Services.Base;
using System.Collections.Generic;
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

        public ResponseBase Detail(int orderId, int? userId)
        {
            try
            {

                Expression<Func<Order, bool>> predicate;

                if (userId.HasValue)
                {
                    predicate = o => o.OrderId == orderId && o.CustomerId == userId;
                }
                else
                {
                    predicate = o => o.OrderId == orderId;
                }

                Order? order = _unitOfWork.OrderRepository.GetSingle(null, predicate);
                if (order == null)
                {
                    return new ResponseBase("Not found order", (int)HttpStatusCode.NotFound);
                }

                List<OrderDetail> orderDetails = order.OrderDetails.ToList();
                List<OrderDetailListDTO> data = _mapper.Map<List<OrderDetailListDTO>>(orderDetails);
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase List(string? status, int pageSize, int currentPage, int? userId)
        {
            try
            {
                Func<IQueryable<Order>, IQueryable<Order>> include = item => item.Include(o => o.Customer);
                List<Expression<Func<Order, bool>>> predicates = new List<Expression<Func<Order, bool>>>();
                if (userId.HasValue)
                {
                    predicates.Add(o => o.CustomerId == userId);
                }

                if (status != null && status.Trim().Length > 0)
                {
                    predicates.Add(o => o.Status == status.Trim());
                }

                Func<IQueryable<Order>, IQueryable<Order>> sort = item => item.OrderBy(o => o.Status == OrderStatus.Pending.ToString() ? 0 : 1)
                .OrderByDescending(o => o.UpdateAt);
                IQueryable<Order> query = _unitOfWork.OrderRepository.GetAll(include, sort, predicates.ToArray());
                List<Order> orders = query.ToList();
                List<OrderListDTO> list = _mapper.Map<List<OrderListDTO>>(orders);

                Pagination<OrderListDTO> data = new Pagination<OrderListDTO>()
                {
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalElement = query.Count(),
                    List = list
                };

                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase> Update(int orderId, OrderUpdateDTO DTO)
        {
            try
            {
                Order? order = _unitOfWork.OrderRepository.GetSingle(item => item.Include(o => o.Customer)
                .Include(o => o.OrderDetails), o => o.OrderId == orderId);
                if (order == null)
                {
                    return new ResponseBase("Not found order", (int)HttpStatusCode.NotFound);
                }

                if(DTO.Status.ToLower().Trim() != OrderStatus.Approved.ToString().ToLower().Trim()
                    || DTO.Status.ToLower().Trim() != OrderStatus.Rejected.ToString().ToLower().Trim() 
                    || DTO.Status.ToLower().Trim() != OrderStatus.Done.ToString().ToLower().Trim()
                    || DTO.Status.ToLower().Trim() != OrderStatus.Ship_Fail.getDescription().ToLower().Trim())
                {
                    return new ResponseBase("Order status invalid", (int)HttpStatusCode.Conflict);
                }

                List<OrderDetail> orderDetails = order.OrderDetails.ToList();
                List<OrderDetailListDTO> data = _mapper.Map<List<OrderDetailListDTO>>(orderDetails);

                //-----------------------  if status update is approved ------------------------------
                if(DTO.Status.ToLower().Trim() == OrderStatus.Approved.ToString().ToLower().Trim())
                {
                    // if order status is approved
                    if(order.Status == OrderStatus.Approved.ToString().ToLower().Trim())
                    {
                        order.UpdateAt = DateTime.Now;
                        order.Note = StringHelper.getStringValue(DTO.Note);

                        _unitOfWork.BeginTransaction();
                        _unitOfWork.OrderRepository.Update(order);
                        _unitOfWork.Commit();
                        return new ResponseBase(data, "Update successful");
                    }

                    // if order status is pending
                    if (order.Status == OrderStatus.Pending.ToString().ToLower().Trim())
                    {
                        order.Status = OrderStatus.Approved.ToString();
                        order.UpdateAt = DateTime.Now;
                        order.Note = StringHelper.getStringValue(DTO.Note);

                        _unitOfWork.BeginTransaction();

                        foreach (OrderDetail detail in orderDetails)
                        {
                            Product? product = _unitOfWork.ProductRepository.GetSingle(null, p => p.ProductId == detail.ProductId && p.IsDeleted == false);
                            if (product == null)
                            {
                                _unitOfWork.RollBack();
                                return new ResponseBase(data, $"Product '{detail.ProductName}' not exist!!!", (int)HttpStatusCode.NotFound);
                            }

                            if (product.Quantity < detail.Quantity)
                            {
                                _unitOfWork.RollBack();
                                return new ResponseBase(data, $"Product '{detail.ProductName}' doesn't have enough quantity!!!", (int)HttpStatusCode.Conflict);
                            }

                            product.Quantity = product.Quantity - detail.Quantity;
                            product.UpdateAt = DateTime.Now;
                            _unitOfWork.ProductRepository.Update(product);
                        }

                        string body = UserHelper.BodyEmailForApproveOrder(orderDetails);
                        await UserHelper.sendEmail("[PHONE SHOPPING] Notification for approve order", body, order.Customer.Email);
                        _unitOfWork.OrderRepository.Update(order);
                        _unitOfWork.Commit();
                        return new ResponseBase(data, "Update successful");
                    }

                    return new ResponseBase(data, $"You can only change status to '{OrderStatus.Approved}' for orders status '{OrderStatus.Pending}'", (int) HttpStatusCode.Conflict);
                }

                //-----------------------  if status update is rejected ------------------------------
                if (DTO.Status.ToLower().Trim() == OrderStatus.Rejected.ToString().ToLower().Trim())
                {
                    // if order status is pending
                    if (order.Status == OrderStatus.Pending.ToString().ToLower().Trim())
                    {
                        // if not note
                        if (StringHelper.isStringNullOrEmpty(DTO.Note))
                        {
                            return new ResponseBase(data, $"You have to note when order '{OrderStatus.Rejected}'", (int) HttpStatusCode.Conflict);
                        }

                        order.Status = OrderStatus.Rejected.ToString();
                        order.UpdateAt = DateTime.Now;
                        order.Note = StringHelper.getStringValue(DTO.Note);

                        _unitOfWork.BeginTransaction();
                        _unitOfWork.OrderRepository.Update(order);
                        _unitOfWork.Commit();
                        return new ResponseBase(data, "Update successful");
                    }

                    return new ResponseBase(data, $"You can only change order status to '{OrderStatus.Rejected}' for orders '{OrderStatus.Pending}'" 
                        + $"When order status is '{OrderStatus.Rejected}', you can't update anymore", (int)HttpStatusCode.Conflict);
                }

                //-----------------------  if status update is done ------------------------------
                if (DTO.Status.ToLower().Trim() == OrderStatus.Done.ToString().ToLower().Trim())
                {

                    // if order status is approved
                    if (order.Status == OrderStatus.Approved.ToString().ToLower().Trim())
                    {
                        order.Status = OrderStatus.Done.ToString();
                        order.UpdateAt = DateTime.Now;
                        order.Note = StringHelper.getStringValue(DTO.Note);

                        _unitOfWork.BeginTransaction();
                        _unitOfWork.OrderRepository.Update(order);
                        _unitOfWork.Commit();
                        return new ResponseBase(data, "Update successful");
                    }

                    return new ResponseBase(data, $"You can only change order status to '{OrderStatus.Done}' for orders '{OrderStatus.Approved}' " +
                        $"And when changing to '{OrderStatus.Done}', you can't update anymore", (int)HttpStatusCode.Conflict);
                }

                //-----------------------  if status update is ship fail ------------------------------
                // if order status is approved
                if (order.Status == OrderStatus.Approved.ToString().ToLower().Trim())
                {

                    // if not note
                    if (StringHelper.isStringNullOrEmpty(DTO.Note))
                    {
                        return new ResponseBase(data, $"You have to note when order '{OrderStatus.Ship_Fail.getDescription()}'", (int)HttpStatusCode.Conflict);
                    }

                    _unitOfWork.BeginTransaction();

                    foreach (OrderDetail detail in orderDetails)
                    {
                        Product? product = _unitOfWork.ProductRepository.GetSingle(null, p => p.ProductId == detail.ProductId && p.IsDeleted == false);
                        if (product == null)
                        {
                            _unitOfWork.RollBack();
                            return new ResponseBase(data, $"Product '{detail.ProductName}' not exist!!!", (int)HttpStatusCode.NotFound);
                        }

                        product.Quantity = product.Quantity + detail.Quantity;
                        product.UpdateAt = DateTime.Now;
                        _unitOfWork.ProductRepository.Update(product);
                    }

                    order.Status = OrderStatus.Ship_Fail.getDescription();
                    order.UpdateAt = DateTime.Now;
                    order.Note = StringHelper.getStringValue(DTO.Note);

                    _unitOfWork.BeginTransaction();
                    _unitOfWork.OrderRepository.Update(order);
                    _unitOfWork.Commit();
                }

                return new ResponseBase(data, $"You can only change order status to '{OrderStatus.Ship_Fail.getDescription()}' for orders '{OrderStatus.Approved}' " +
                    $"And when changing to '{OrderStatus.Ship_Fail.getDescription()}', you can't update anymore", (int)HttpStatusCode.Conflict);

            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}