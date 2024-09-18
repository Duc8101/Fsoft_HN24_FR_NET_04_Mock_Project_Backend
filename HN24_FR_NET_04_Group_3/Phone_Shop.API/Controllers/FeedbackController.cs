using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phone_Shop.API.Attributes;
using Phone_Shop.Common.DTOs.FeedbackDTO;
using Phone_Shop.Common.Enums;
using Phone_Shop.Common.Responses;
using Phone_Shop.Services.Feedbacks;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Phone_Shop.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class FeedbackController : BaseAPIController
    {

        private readonly IFeedbackService _service;

        public FeedbackController(IFeedbackService service)
        {
            _service = service;
        }

        [HttpPost("create-feedback")]
        [Role(Roles.Customer)]
        public ResponseBase Create([Required] FeedbackCreateDTO DTO)
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

            return _service.Create(DTO, userId);
        }

        [HttpPost("reply-feedback")]
        [Role(Roles.Staff)]
        public ResponseBase Reply([Required] FeedbackReplyDTO DTO)
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

            return _service.Reply(DTO, userId);
        }
    }
}
