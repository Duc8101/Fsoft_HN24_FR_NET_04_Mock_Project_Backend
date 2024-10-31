using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phone_Shop.API.Attributes;
using Phone_Shop.Common.DTOs.OrderDTO;
using Phone_Shop.Common.Enums;
using Phone_Shop.Common.Responses;
using Phone_Shop.Services.Orders;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Phone_Shop.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class OrderController : BaseAPIController
    {

        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        [Role(Roles.Customer)]
        public async Task<ResponseBase> Create([Required] OrderCreateDTO DTO)
        {
            string? UserId = GetUserId();
            if (UserId == null)
            {
                return new ResponseBase($"Not found user id login", (int)HttpStatusCode.NotFound);
            }

            if (!int.TryParse(UserId, out int userId))
            {
                return new ResponseBase($"User id login {UserId} not valid", (int)HttpStatusCode.Conflict);
            }

            return await _service.Create(DTO, userId);
        }

        [HttpGet]
        public ResponseBase List(string? status, [Required] int pageSize = 10, [Required] int currentPage = 1)
        {
            if (IsAdmin())
            {
                return _service.List(status, pageSize, currentPage, null);
            }

            string? UserId = GetUserId();
            if (UserId == null)
            {
                return new ResponseBase($"Not found user id login", (int)HttpStatusCode.NotFound);
            }

            if (!int.TryParse(UserId, out int userId))
            {
                return new ResponseBase($"User id login {UserId} not valid", (int)HttpStatusCode.Conflict);
            }

            return _service.List(status, pageSize, currentPage, userId);
        }

        [HttpGet("{orderId}")]
        public ResponseBase Detail([Required] int orderId)
        {
            if (IsAdmin())
            {
                return _service.Detail(orderId, null);
            }

            string? UserId = GetUserId();
            if (UserId == null)
            {
                return new ResponseBase($"Not found user id login", (int)HttpStatusCode.NotFound);
            }

            if (!int.TryParse(UserId, out int userId))
            {
                return new ResponseBase($"User id login {UserId} not valid", (int)HttpStatusCode.Conflict);
            }

            return _service.Detail(orderId, userId);
        }

        [HttpPut("{orderId}")]
        [Role(Roles.Admin)]
        public async Task<ResponseBase> Update([Required] int orderId, [Required] OrderUpdateDTO DTO)
        {
            return await _service.Update(orderId, DTO);
        }
    }
}
