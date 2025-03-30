using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;

namespace ANF.Core.Services
{
    public interface IPolicyService
    {
        Task<PaginationResponse<PolicyResponse>> GetPolicies(PaginationRequest request);
        Task<PolicyResponse> GetPolicyById(long policyId);
        Task<bool> CreatePolicy(PolicyCreateRequest request);
        Task<bool> UpdatePolicy(long id, PolicyUpdateRequest request);
        Task<bool> DeletePolicy(long id);
    }
}
