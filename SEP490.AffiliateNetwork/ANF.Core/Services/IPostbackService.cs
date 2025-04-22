using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;

namespace ANF.Core.Services
{
    public interface IPostbackService
    {
        public Task<bool> CreatePostBack(PostbackRequest postbackRequest);
        public Task<bool> CreatePurchaseLog(PurchaseLogRequest purchaseLogRequest);
        public Task<bool> UpdatePostBackStatus(long id, string status);
        public Task<bool> UpdatePostBackLog(string id, PostbackLogUpdateRequest request);
        public Task<PostbackData> GetPostbackDataByTransactionId(string id);
        public Task<List<PurchaseLog>> GetAllPostbackLogByClickId(string id);
        public Task<List<PurchaseLog>> GetAllPostbackLogByTransactionId(string id);
        public Task<PurchaseLog> GetPostbackLogById(long id);
    }
}
