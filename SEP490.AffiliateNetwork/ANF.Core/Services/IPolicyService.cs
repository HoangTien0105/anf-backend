using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANF.Core.Services
{
    public interface IPolicyService
    {
        Task<PaginationResponse<PolicyResponse>> GetPolicies(PaginationRequest request);
        Task<PolicyResponse> GetPolicyById(long policyId);
        Task<PolicyResponse> CreatePolicy(PolicyCreateRequest request);
        Task<bool> UpdatePolicy(long id, PolicyUpdateRequest request);
        Task<bool> DeletePolicy(long id);
    }
}
