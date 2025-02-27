using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;

namespace ANF.Core.Services
{
    public interface ICategoryService
    {
        Task<PaginationResponse<CategoryResponse>> GetCategories(PaginationRequest request);

        Task<CategoryResponse> GetCategory(long id);

        Task<bool> AddCategory(CategoryCreateRequest request);

        Task<bool> UpdateCategory(long id, CategoryUpdateRequest request);

        Task<bool> DeleteCategory(long id);
    }
}
