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
    public class FeedbackController : BaseAPIController
    {

        private readonly IFeedbackService _service;

        public FeedbackController(IFeedbackService service)
        {
            _service = service;
        }

        [HttpPost("create-feedback")]
        [Authorize]
        [Role(Roles.Customer)]
        public ResponseBase Create([Required] FeedbackCreateDTO DTO)
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

        [HttpPost("reply-feedback")]
        [Authorize]
        [Role(Roles.Admin)]
        public ResponseBase Reply([Required] FeedbackReplyDTO DTO)
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

            return _service.Reply(DTO, userId);
        }

        [HttpGet("get-feedbacks-by-product-id")]
        public ResponseBase GetFeedbacksByProductId([Required] int productId)
        {
            return _service.GetFeedbacksByProductId(productId);
        }

        [HttpGet("get-feedbacks-by-order-detail-id")]
        [Authorize]
        public ResponseBase GetFeedbacksByOrderDetailId([Required] int orderDetailId)
        {
            return _service.GetFeedbacksByOrderDetailId(orderDetailId);
        }
    }
}
