using Microsoft.AspNetCore.Mvc;
using Phone_Shop.Common.Enums;
using System.Security.Claims;

namespace Phone_Shop.API.Controllers
{

    public class BaseAPIController : ControllerBase
    {

        private Claim? GetClaim(string type)
        {
            return User.Claims.FirstOrDefault(c => c.Type == type);
        }

        private protected string? GetUserId()
        {
            Claim? claim = GetClaim("id");
            return claim?.Value;
        }

        private protected string? GetUsername()
        {
            Claim? claim = GetClaim("username");
            return claim?.Value;
        }

        private protected bool IsAdmin()
        {
            Claim? claim = GetClaim(ClaimTypes.Role);
            return claim != null && claim.Value == Roles.Admin.ToString();
        }
    }
}
