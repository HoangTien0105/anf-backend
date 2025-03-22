using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;

namespace ANF.Core.Services
{
    public interface IUserService
    {
        Task<LoginResponse> Login(string email, string password);

        Task<bool> RegisterAccount(AccountCreateRequest request);

        Task<UserStatusResponse> UpdateAccountStatus(long userId, string status);

        Task<PaginationResponse<AdvertiserResponse>> GetAdvertisers(PaginationRequest request);

        Task<PaginationResponse<PublisherResponse>> GetPublishers(PaginationRequest request);

        Task<bool> DeleteUser(long id);

        Task<bool> ChangeEmailStatus(long userId);

        Task<bool> ChangePassword(string email);

        Task<bool> UpdatePassword(string token, long userId, UpdatePasswordRequest request);

        Task<bool> ActivateWallet(string userCode);

        Task<DetailedUserResponse> GetUserInformation();
    }
}
