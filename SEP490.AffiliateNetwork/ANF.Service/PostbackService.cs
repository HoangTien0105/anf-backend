using ANF.Core;
using ANF.Core.Enums;
using ANF.Core.Exceptions;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ANF.Service
{
    public class PostbackService(IUnitOfWork unitOfWork, IMapper mapper) : IPostbackService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> CreatePostBack(PostbackRequest postbackRequest)
        {
            try
            {
                if(postbackRequest is null) throw new NullReferenceException("Invalid request data. Please check again!");
                
                var postbackRepository = _unitOfWork.GetRepository<PostbackData>();
                var offerRepository = _unitOfWork.GetRepository<Offer>();
                var trackingEventRepository = _unitOfWork.GetRepository<TrackingEvent>();

                var trackingEvent = await trackingEventRepository.GetAll().AsNoTracking().Include(e => e.Offer).FirstOrDefaultAsync(e => e.Id == postbackRequest.ClickId);
                if(trackingEvent is null) throw new KeyNotFoundException("Tracking event does not exist.");

                var postbackExist = await postbackRepository.GetAll().AnyAsync(e => e.ClickId == postbackRequest.ClickId);
                if (postbackExist) throw new DuplicatedException("A postback already exists for this tracking event.");

                if (!Enum.TryParse<PostbackStatus>(postbackRequest.Status, true, out var status))
                    throw new ArgumentException("Invalid postback's status. Please check again!");

                var postbackData = _mapper.Map<PostbackData>(postbackRequest);

                int date = 7;

                if(trackingEvent.Offer is not null && trackingEvent.Offer.OrderReturnTime is not null)
                {
                    var parts = trackingEvent.Offer.OrderReturnTime.Trim().Split(" ");
                    int.TryParse(parts[0], out date);
                }
                postbackData.Date = DateTime.Now.AddDays(date);

                postbackData.OfferId = trackingEvent.OfferId;
                postbackData.PublisherCode = trackingEvent.PublisherCode;

                postbackRepository.Add(postbackData);
                var affectedRows = await _unitOfWork.SaveAsync();
                return affectedRows > 0;

            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdatePostBackStatus(long id, string status)
        {
            try
            {
                var postbackRepository = _unitOfWork.GetRepository<PostbackData>();
                var postback = await postbackRepository.GetAll().FirstOrDefaultAsync(e => e.Id == id);

                if(postback is null) throw new KeyNotFoundException("Tracking event does not exists");

                if (!Enum.TryParse<PostbackStatus>(status, true, out var postbackStatus))
                    throw new ArgumentException("Invalid postback's status. Please check again!");

                if(DateTime.Now > postback.Date)
                    throw new ArgumentException("This postback cannot be modified anymore");

                postback.Status = postbackStatus;

                postbackRepository.Update(postback);

                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
