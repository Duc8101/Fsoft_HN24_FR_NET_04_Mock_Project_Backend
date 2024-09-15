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
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : BaseAPIController
    {

        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost("[action]")]
        [Role(Roles.Customer)]
        public async Task<ResponseBase> Create([Required] OrderCreateDTO DTO)
        {
            string? UserId = getUserId();
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
    }
}
