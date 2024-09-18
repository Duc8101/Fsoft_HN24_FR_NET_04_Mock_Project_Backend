using Common.DTOs.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phone_Shop.Common.DTOs.UserDTO;
using Phone_Shop.Common.Responses;
using Phone_Shop.Services.Users;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Phone_Shop.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : BaseAPIController
    {

        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }


        [HttpPost("[action]")]
        public ResponseBase Login([Required] LoginDTO DTO)
        {
            return _service.Login(DTO);
        }

        [HttpPost("[action]")]
        public ResponseBase Register([Required] RegisterDTO DTO)
        {
            return _service.Register(DTO);
        }

        [HttpPut("forgot-password")]
        public async Task<ResponseBase> ForgotPassword([Required] ForgotPasswordDTO DTO)
        {
            return await _service.ForgotPassword(DTO);
        }

        [HttpGet("[action]")]
        [Authorize]
        public ResponseBase Detail()
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

            return _service.Detail(userId);
        }

        [HttpPut("update-profile")]
        [Authorize]
        public ResponseBase UpdateProfile([Required] ProfileDTO DTO)
        {
            string? UserId = getUserId();
            if(UserId == null)
            {
                return new ResponseBase($"Not found user id login", (int) HttpStatusCode.NotFound);
            }

            if(!int.TryParse(UserId, out int userId))
            {
                return new ResponseBase($"User id login {UserId} not valid", (int)HttpStatusCode.Conflict);
            }

            return _service.UpdateProfile(DTO, userId);
        }

        [HttpPut("change-password")]
        [Authorize]
        public ResponseBase ChangePassword([Required] ChangePasswordDTO DTO)
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

            return _service.ChangePassword(DTO, userId);
        }

        [HttpGet("get-user-by-token")]
        public ResponseBase GetUserByToken([Required] string token)
        {
            return _service.GetUserByToken(token);
        }

        [HttpGet("[action]")]
        [Authorize]
        public ResponseBase Logout()
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

            return _service.Logout(userId);
        }
    }
}
