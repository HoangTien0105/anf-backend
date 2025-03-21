using ANF.Core;
using ANF.Core.Exceptions;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ANF.Service
{
    public class SubscriptionService(IUnitOfWork unitOfWork,
                                     IMapper mapper) : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> CreateSubscription(SubscriptionRequest request)
        {
            try
            {
                var subscriptionRepository = _unitOfWork.GetRepository<Subscription>();
                if(request is null) throw new NullReferenceException("Invalid request data. Please check again!");
                var duplicatedSub = await subscriptionRepository.GetAll()
                                        .AsNoTracking()
                                        .AnyAsync(e => e.Name == request.Name && e.Description.Trim() == request.Description.Trim());
                if (duplicatedSub) throw new DuplicatedException("Subscription already exists");

                var subscription = _mapper.Map<Subscription>(request);
                subscriptionRepository.Add(subscription);
                var affectedRows = await _unitOfWork.SaveAsync();
                return affectedRows > 0;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteSubscriptions(long id)
        {
            try
            {
                var subscriptionRepository = _unitOfWork.GetRepository<Subscription>();
                var subscription = await subscriptionRepository.GetAll()
                    .AsNoTracking()
                    .Include(u => u.Transactions)
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (subscription is not null)
                {
                    if (subscription.Transactions.Any())
                        throw new InvalidOperationException("Subscription already has purchases.");
                    subscriptionRepository.Delete(subscription);
                    return await _unitOfWork.SaveAsync() > 0;
                }
                else
                {
                    throw new KeyNotFoundException("Subscription does not exist!");
                }
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<SubscriptionResponse> GetSubscription(long subscriptionId)
        {
            var subscriptionRepository = _unitOfWork.GetRepository<Subscription>();
            var subscription = await subscriptionRepository.FindByIdAsync(subscriptionId);
            if (subscription is null)
                throw new KeyNotFoundException("Subscription does not exist!");
            var response = _mapper.Map<SubscriptionResponse>(subscription);
            return response;
        }

        public async Task<PaginationResponse<SubscriptionResponse>> GetSubscriptions(PaginationRequest request)
        {
            var subscriptionRepository = _unitOfWork.GetRepository<Subscription>();
            var subscriptions = await subscriptionRepository.GetAll()
                            .AsNoTracking()
                            .Skip((request.pageNumber - 1) * request.pageSize)
                            .Take(request.pageSize)
                            .ToListAsync();
            if (!subscriptions.Any())
                throw new KeyNotFoundException("No data for subscriptions!");
            var totalCounts = subscriptions.Count();

            var data = _mapper.Map<List<SubscriptionResponse>>(subscriptions);
            return new PaginationResponse<SubscriptionResponse>(data, totalCounts, request.pageNumber, request.pageSize);
        }

        public async Task<bool> UpdateSubscription(long id, SubscriptionRequest request)
        {
            try
            {
                if (request is null) throw new NullReferenceException("Invalid request data. Please check again!");
                var subscriptionRepository = _unitOfWork.GetRepository<Subscription>();
                var subscription = await subscriptionRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (subscription is not null)
                {
                    var duplicatedSub = await subscriptionRepository.GetAll()
                        .AsNoTracking()
                        .AnyAsync(e => e.Name == request.Name && e.Description.Trim() == request.Description.Trim() && e.Id != id);
                    if (duplicatedSub) throw new DuplicatedException("Description already exists");

                    subscription.Name = request.Name;
                    subscription.Description = request.Description;
                    subscription.Price = (decimal)Math.Floor(request.Price); 
                    subscription.Duration = request.Duration;

                    subscriptionRepository.Update(subscription);
                    var affectedRows = await _unitOfWork.SaveAsync();
                    return affectedRows > 0;
                }
                else
                {
                    throw new KeyNotFoundException("Subscription does not exist!");
                }
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
