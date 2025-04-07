using ANF.Core;
using ANF.Core.Enums;
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
                var trackingEventRepository = _unitOfWork.GetRepository<TrackingEvent>();

                var trackingEvent = await trackingEventRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.Id == postbackRequest.ClickId);
                if(trackingEvent is null) throw new KeyNotFoundException("Tracking event does not exists");

                if (!Enum.TryParse<PostbackStatus>(postbackRequest.Status, true, out var status))
                    throw new ArgumentException("Invalid postback's status. Please check again!");

                var postbackData = _mapper.Map<PostbackData>(postbackRequest);
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
    }
}
