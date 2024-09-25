using Phone_Shop.Common.DTOs.FeedbackDTO;
using Phone_Shop.Common.Responses;

namespace Phone_Shop.Services.Feedbacks
{
    public interface IFeedbackService
    {
        ResponseBase Create(FeedbackCreateDTO DTO, int creatorId);
        ResponseBase Reply(FeedbackReplyDTO DTO, int creatorId);
        ResponseBase GetFeedbacksByProductId(int productId);
        ResponseBase GetFeedbacksByOrderDetailId(int orderDetailId);
    }
}
