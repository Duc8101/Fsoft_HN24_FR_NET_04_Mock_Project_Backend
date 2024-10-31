using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phone_Shop.API.Attributes;
using Phone_Shop.Common.DTOs.CartDTO;
using Phone_Shop.Common.Enums;
using Phone_Shop.Common.Responses;
using Phone_Shop.Services.Carts;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Phone_Shop.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    [Role(Roles.Customer)]
    public class CartController : BaseAPIController
    {

        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpGet]
        public ResponseBase List()
        {
            string? UserId = GetUserId();
            if (UserId == null)
            {
                return new ResponseBase($"Not found user id login", (int)HttpStatusCode.NotFound);
            }

            string? username = GetUsername();
            if (username == null)
            {
                return new ResponseBase($"Not found username login", (int)HttpStatusCode.NotFound);
            }

            if (!int.TryParse(UserId, out int userId))
            {
                return new ResponseBase($"User id login {UserId} not valid", (int)HttpStatusCode.Conflict);
            }

            return _service.List(userId, username);
        }

        [HttpPost]
        public ResponseBase Create([Required] CartCreateDTO DTO)
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

            return _service.Create(DTO, userId);
        }

        [HttpDelete]
        public ResponseBase Delete([Required] int productId)
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

            return _service.Delete(productId, userId);
        }

        [HttpPut]
        public ResponseBase Update([Required] CartUpdateDTO DTO)
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

            return _service.Update(DTO, userId);
        }
    }
}
