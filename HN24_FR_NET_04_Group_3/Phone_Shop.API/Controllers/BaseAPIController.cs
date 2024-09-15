using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Phone_Shop.API.Controllers
{

    public class BaseAPIController : ControllerBase
    {
        private Claim? getClaim(string type)
        {
            return User.Claims.FirstOrDefault(c => c.Type == type);
        }

        private protected string? getUserId()
        {
            Claim? claim = getClaim("id");
            return claim?.Value;
        }

        private protected string? getUsername()
        {
            Claim? claim = getClaim("username");
            return claim?.Value;
        }
    }
}
