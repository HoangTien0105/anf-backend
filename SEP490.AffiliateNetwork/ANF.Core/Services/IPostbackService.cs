using ANF.Core.Models.Requests;

namespace ANF.Core.Services
{
    public interface IPostbackService
    {
        public Task<bool> CreatePostBack(PostbackRequest postbackRequest);
        public Task<bool> UpdatePostBackStatus(long id, string status);
    }
}
