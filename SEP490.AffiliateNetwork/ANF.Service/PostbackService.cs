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

                postbackData.Date = DateTime.Now;

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

        public async Task<bool> CreatePurchaseLog(PurchaseLogRequest purchaseLogRequest)
        {
            try
            {
                var purchaseLogRepository = _unitOfWork.GetRepository<PurchaseLog>();

                var logExist = await purchaseLogRepository.GetAll().AsNoTracking().AnyAsync(e => e.ClickId == purchaseLogRequest.ClickId);

                if(logExist) throw new DuplicatedException("A postback logs already exists for this tracking event.");

                var purchaseLog = _mapper.Map<PurchaseLog>(purchaseLogRequest);
                purchaseLogRepository.Add(purchaseLog);
                var affectedRows = await _unitOfWork.SaveAsync();
                return affectedRows > 0;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<List<PurchaseLog>> GetAllPostbackLogByClickId(string id)
        {
            var postbackLogRepository = _unitOfWork.GetRepository<PurchaseLog>();

            var postbackLog = await postbackLogRepository.GetAll().AsNoTracking().Where(e => e.ClickId == id).ToListAsync();
            if(!postbackLog.Any()) throw new KeyNotFoundException("No data for postback logs!");
            return postbackLog;
        }

        public async Task<List<PurchaseLog>> GetAllPostbackLogByTransactionId(string id)
        {
            var postbackLogRepository = _unitOfWork.GetRepository<PurchaseLog>();

            var postbackLog = await postbackLogRepository.GetAll().AsNoTracking().Where(e => e.TransactionId == id).ToListAsync();
            if (!postbackLog.Any()) throw new KeyNotFoundException("No data for postback logs!");
            return postbackLog;
        }

        public async Task<PurchaseLog> GetPostbackLogById(long id)
        {
            var postbackLogRepository = _unitOfWork.GetRepository<PurchaseLog>();

            var postbackLog = await postbackLogRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (postbackLog is null) throw new KeyNotFoundException("No data for postback logs!");
            return postbackLog;
        }

        public async Task<bool> UpdatePostBackLog(string id, PostbackLogUpdateRequest request)
        {
            try
            {
                var postbackLogRepository = _unitOfWork.GetRepository<PurchaseLog>();
                var trackingValidationRepository = _unitOfWork.GetRepository<TrackingValidation>();

                var postbackLog = await postbackLogRepository.GetAll().FirstOrDefaultAsync(e => e.ClickId == id);
                if (postbackLog is null) throw new KeyNotFoundException("Postback log not found!");

                var trackingValidation = await trackingValidationRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.ClickId == postbackLog.ClickId);
                if (trackingValidation is null) throw new KeyNotFoundException("This postback is not valid");

                if (trackingValidation.ConversionStatus != ConversionStatus.Pending)
                    throw new ArgumentException("This postback's tracking has already been resolved.");

                _ = _mapper.Map(request, postbackLog);

                postbackLogRepository.Update(postbackLog);
                return await _unitOfWork.SaveAsync() > 0;
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
