using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Phone_Shop.Common.Entity;
using Phone_Shop.Common.Enums;
using Phone_Shop.Common.Responses;
using Phone_Shop.DataAccess.Repositories.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace Phone_Shop.API.Attributes
{
    public class RoleAttribute : Attribute, IActionFilter
    {

        private readonly Roles[] _roles;

        public RoleAttribute(params Roles[] roles)
        {
            _roles = roles;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            IRepository<User> userRepository = context.HttpContext.RequestServices.GetRequiredService<IRepository<User>>();

            // get token
            string? token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken security = handler.ReadJwtToken(token);
            string? UserId = security.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            ResponseBase response;
            if (UserId == null)
            {
                response = new ResponseBase("Not found user id login. Please check login information", (int)HttpStatusCode.NotFound);
                context.Result = new JsonResult(response)
                {
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }
            else if (!int.TryParse(UserId, out int userId))
            {
                response = new ResponseBase("User id login invalid. Please check login information", (int)HttpStatusCode.Conflict);
                context.Result = new JsonResult(response)
                {
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }
            else
            {
                User? user = userRepository.FindById(userId);
                if (user == null)
                {
                    response = new ResponseBase($"Not found user with id = {userId}. Please check login information", (int)HttpStatusCode.NotFound);
                    context.Result = new JsonResult(response)
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                    };
                }
                else if(_roles.Contains((Roles) user.RoleId) == false)
                {
                    response = new ResponseBase($"You are not allowed to access", (int)HttpStatusCode.Forbidden);
                    context.Result = new JsonResult(response)
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                    };

                }
            }
        }
    }
}
