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
    public class CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> AddCategory(CategoryCreateRequest request)
        {
            try
            {
                var categoryRepository = _unitOfWork.GetRepository<Category>();
                if (request is null)
                {
                    throw new NullReferenceException("Invalid request data. Please check again!");
                }
                var existedCategory = await categoryRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == request.Name.ToLower());
                if (existedCategory is not null)
                {
                    throw new DuplicatedException("Category already existed!");
                }
                var category = _mapper.Map<Category>(request);
                categoryRepository.Add(category);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteCategory(long id)
        {
            try
            {
                var categoryRepository = _unitOfWork.GetRepository<Category>();
                var category = await categoryRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (category is null)
                {
                    throw new KeyNotFoundException("Category does not exist.");
                }
                //WARNING: Conflict foreign key relationship.
                categoryRepository.Delete(category);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<PaginationResponse<CategoryResponse>> GetCategories(PaginationRequest request)
        {
            var categoryRepository = _unitOfWork.GetRepository<Category>();
            
            var query = categoryRepository.GetAll()
                .AsNoTracking();
            var totalRecord = await query.CountAsync();
            var categories = await query
                .Skip((request.pageNumber - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();
            if (!categories.Any())
                throw new NoDataRetrievalException("No data of categories.");
            
            var data = _mapper.Map<List<CategoryResponse>>(categories);
            return new PaginationResponse<CategoryResponse>(data, totalRecord, request.pageNumber, request.pageSize);
        }

        public async Task<CategoryResponse> GetCategory(long id)
        {
            var categoryRepository = _unitOfWork.GetRepository<Category>();
            var category = await categoryRepository.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
                throw new KeyNotFoundException("Category does not exist!");
            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<bool> UpdateCategory(long id, CategoryUpdateRequest request)
        {
            try
            {
                var categoryRepository = _unitOfWork.GetRepository<Category>();
                if (request is null)
                    throw new NullReferenceException("Invalid request data. Please check again!");
                var category = await categoryRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (category is null)
                {
                    throw new KeyNotFoundException("Category does not exist.");
                }
                _ = _mapper.Map(request, category);
                categoryRepository.Update(category);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
