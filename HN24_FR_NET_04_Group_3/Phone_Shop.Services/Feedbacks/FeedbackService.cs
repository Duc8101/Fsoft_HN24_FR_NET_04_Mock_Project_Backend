﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Phone_Shop.Common.DTOs.FeedbackDTO;
using Phone_Shop.Common.Enums;
using Phone_Shop.Common.Responses;
using Phone_Shop.DataAccess.Entity;
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
                    return new ResponseBase($"This order doesn't belong to you", (int)HttpStatusCode.Conflict);
                }

                if (detail.Order.Status != OrderStatus.Done.ToString())
                {
                    return new ResponseBase($"You can only create feedback when your order status is '${OrderStatus.Done}'", (int)HttpStatusCode.Conflict);
                }

                if (DTO.Comment.Trim().Length == 0)
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
                _unitOfWork.Rollback();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }

        }

        public ResponseBase GetFeedbacksByOrderDetailId(int orderDetailId)
        {
            try
            {
                Func<IQueryable<Feedback>, IQueryable<Feedback>> include = item => item.Include(f => f.Creator).Include(f => f.Reply)
                .ThenInclude(f => f!.Creator);

                Func<IQueryable<Feedback>, IQueryable<Feedback>> sort = item => item.OrderByDescending(f => f.CreatedAt);

                IQueryable<Feedback> query = _unitOfWork.FeedbackRepository.GetAll(include, sort, f => f.OrderDetailId == orderDetailId);
                List<FeedbackListDTO> data = query.Select(f => _mapper.Map<FeedbackListDTO>(f)).ToList();
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase GetFeedbacksByProductId(int productId)
        {
            try
            {
                Func<IQueryable<Feedback>, IQueryable<Feedback>> include = item => item.Include(f => f.Creator).Include(f => f.Reply)
                .ThenInclude(f => f!.Creator);

                Func<IQueryable<Feedback>, IQueryable<Feedback>> sort = item => item.OrderByDescending(f => f.CreatedAt);

                IQueryable<Feedback> query = _unitOfWork.FeedbackRepository.GetAll(include, sort, f => f.OrderDetail.ProductId == productId && f.Rate.HasValue);
                List<FeedbackListDTO> data = query.Select(f => _mapper.Map<FeedbackListDTO>(f)).ToList();
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
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

                if (DTO.Comment.Trim().Length == 0)
                {
                    return new ResponseBase("Comment not empty", (int)HttpStatusCode.Conflict);
                }

                Feedback feedbackCreate = _mapper.Map<Feedback>(DTO);
                feedbackCreate.CreatorId = creatorId;
                feedbackCreate.ReplyId = DTO.RepliedFeedbackId;
                feedbackCreate.CreatedAt = DateTime.Now;
                feedbackCreate.UpdateAt = DateTime.Now;
                feedbackCreate.OrderDetailId = repliedFeedback.OrderDetailId;
                feedbackCreate.Rate = null;

                _unitOfWork.BeginTransaction();
                _unitOfWork.FeedbackRepository.Add(feedbackCreate);
                _unitOfWork.Commit();
                return new ResponseBase(true, "Reply feedback successful");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
