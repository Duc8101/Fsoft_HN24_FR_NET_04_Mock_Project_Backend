using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Common.DTOs.FeedbackDTO;
using Phone_Shop.Common.Entity;
using Phone_Shop.Common.Enums;
using Phone_Shop.Common.Responses;
using Phone_Shop.DataAccess.Helper;
using Phone_Shop.DataAccess.UnitOfWorks;
using Phone_Shop.Services.Base;
using System.Net;

namespace Phone_Shop.Services.Feedbacks
{
    public class FeedbackService : BaseService, IFeedbackService
    {
        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public ResponseBase Create(FeedbackCreateDTO DTO, int creatorId)
        {
            try
            {
                OrderDetail? detail = _unitOfWork.OrderDetailRepository.GetSingle(item => item.Include(od => od.Order), od => od.OrderDetailId == DTO.OrderDetailId);
                if (detail == null)
                {
                    return new ResponseBase($"Not found order detail with id = {DTO.OrderDetailId}", (int)HttpStatusCode.NotFound);
                }

                if (detail.Order.CustomerId != creatorId)
                {
                    return new ResponseBase($"Order detail with id = {DTO.OrderDetailId} doesn't belong to you", (int)HttpStatusCode.Conflict);
                }

                if (StringHelper.isStringNullOrEmpty(DTO.Comment))
                {
                    return new ResponseBase("Comment not empty", (int)HttpStatusCode.Conflict);
                }

                if (DTO.Rate < (int)FeedBackRate.Min || DTO.Rate > (int)FeedBackRate.Max)
                {
                    return new ResponseBase($"Feedback rate from {(int)FeedBackRate.Min} to {(int)FeedBackRate.Max}", (int)HttpStatusCode.Conflict);
                }

                Feedback feedback = _mapper.Map<Feedback>(DTO);
                feedback.CreatorId = creatorId;
                feedback.ReplyId = null;
                feedback.CreatedAt = DateTime.Now;
                feedback.UpdateAt = DateTime.Now;
                _unitOfWork.BeginTransaction();
                _unitOfWork.FeedbackRepository.Add(feedback);
                _unitOfWork.Commit();
                return new ResponseBase(true, "Create feedback successful");
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }

        }

        public ResponseBase Reply(FeedbackReplyDTO DTO, int creatorId)
        {
            try
            {
                Feedback? repliedFeedback = _unitOfWork.FeedbackRepository.FindById(DTO.RepliedFeedbackId);
                if (repliedFeedback == null)
                {
                    return new ResponseBase($"Not found replied feedback with id = {DTO.RepliedFeedbackId}", (int)HttpStatusCode.NotFound);
                }

                if (StringHelper.isStringNullOrEmpty(DTO.Comment))
                {
                    return new ResponseBase("Comment not empty", (int)HttpStatusCode.Conflict);
                }

                Feedback feedbackCreate = _mapper.Map<Feedback>(DTO);
                feedbackCreate.CreatorId = creatorId;
                feedbackCreate.ReplyId = null;
                feedbackCreate.CreatedAt = DateTime.Now;
                feedbackCreate.UpdateAt = DateTime.Now;
                feedbackCreate.OrderDetailId = repliedFeedback.OrderDetailId;
                feedbackCreate.Rate = null;

                _unitOfWork.BeginTransaction();
                // create feedbackCreate
                _unitOfWork.FeedbackRepository.Add(feedbackCreate);

                // update feedbackReplied
                repliedFeedback.ReplyId = feedbackCreate.FeedbackId;
                repliedFeedback.UpdateAt = DateTime.Now;
                _unitOfWork.FeedbackRepository.Update(repliedFeedback);

                _unitOfWork.Commit();
                return new ResponseBase(true, "Reply feedback successful");
            }
            catch (Exception ex)
            {
                _unitOfWork.RollBack();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
