using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANF.Core.Services
{
    public interface ISubscriptionService
    {
        Task<PaginationResponse<SubscriptionResponse>> GetSubscriptions(PaginationRequest request);
        Task<SubscriptionResponse> GetSubscription(long subscriptionId);
        Task<bool> CreateSubscription(SubscriptionRequest request);
        Task<bool> UpdateSubscription(long id, SubscriptionRequest request);
        Task<bool> DeleteSubscriptions(long id);
        
    }
}
