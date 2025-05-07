using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;

namespace ANF.Core.Services
{
    public interface IFraudDetectionService
    {
        Task<PaginationResponse<FraudDetectionResponse>> GetFraudDetections(PaginationRequest request, DateTime from, DateTime to);
    }
}
