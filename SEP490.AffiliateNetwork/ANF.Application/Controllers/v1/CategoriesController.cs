using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ANF.Core.Models.Requests;
using ANF.Core.Services;
using ANF.Core.Commons;
using ANF.Core.Models.Responses;
using Asp.Versioning;

namespace ANF.Application.Controllers.v1
{
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get categories
        /// </summary>
        /// <param name="request">Pagination model</param>
        /// <returns></returns>
        [HttpGet("categories")]
        [Authorize]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategories([FromQuery] PaginationRequest request)
        {
            var data = await _categoryService.GetCategories(request);
            return Ok(new ApiResponse<PaginationResponse<CategoryResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = data
            });
        }

        /// <summary>
        /// Get category
        /// </summary>
        /// <param name="id">Category's id</param>
        /// <returns></returns>
        [HttpGet("categories/{id}")]
        [Authorize(Roles = "Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategory(long id)
        {
            var category = await _categoryService.GetCategory(id);
            return Ok(new ApiResponse<CategoryResponse>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = category
            });
        }
        
        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="id">Category's id</param>
        /// <param name="category">Data to update existed category</param>
        /// <returns></returns>
        [HttpPut("category/{id}")]
        [Authorize(Roles = "Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(long id, [FromBody] CategoryUpdateRequest category)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var result = await _categoryService.UpdateCategory(id, category);
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success."
            });
        }

        /// <summary>
        /// Create category
        /// </summary>
        /// <param name="request">Model to create category</param>
        /// <returns></returns>
        [HttpPost("category")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateRequest request)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var result = await _categoryService.AddCategory(request);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success."
            });
        }

        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="id">Category's id</param>
        /// <returns></returns>
        [HttpDelete("category/{id}")]
        [Authorize(Roles = "Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            var result = await _categoryService.DeleteCategory(id);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success."
            });
        }
        
    }
}
