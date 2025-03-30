using ANF.Core.Commons;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    public class PolicyController(IPolicyService policyService) : BaseApiController
    {
        private readonly IPolicyService _policyService = policyService;

        /// <summary>
        /// Create ppolicy
        /// </summary>
        /// <param name="request">policy data</param>
        /// <returns></returns>
        [HttpPost("policy")]
        [Authorize(Roles = "Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePolicy([FromBody] PolicyCreateRequest request)
        {

            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var result = await _policyService.CreatePolicy(request);
            if (!result) return BadRequest();

            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Create successfully",
            });
        }
        /// <summary>
        /// Get all policies
        /// </summary>
        /// <param name="request">Pagination request model</param>
        /// <returns></returns>
        [HttpGet("policies")]
        [AllowAnonymous]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPolicies([FromQuery] PaginationRequest request)
        {
            var policies = await _policyService.GetPolicies(request);
            return Ok(new ApiResponse<PaginationResponse<PolicyResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = policies
            });
        }

        /// <summary>
        /// Get policy by id
        /// </summary>
        /// <param name="id">Policy id</param>
        /// <returns></returns>
        [HttpGet("policy/{id}")]
        [AllowAnonymous]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPolicyById(long id)
        {
            var policy = await _policyService.GetPolicyById(id);
            return Ok(new ApiResponse<PolicyResponse>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = policy
            });
        }
        /// <summary>
        /// Update policy
        /// </summary>
        /// <param name="id">policy id</param>
        /// <param name="request">policy data</param>
        /// <returns></returns>
        [HttpPut("policy/{id}")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePolicy(long id, PolicyUpdateRequest request)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }

            var result = await _policyService.UpdatePolicy(id, request);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Update successfully"
            });
        }

        /// <summary>
        /// Delete policy
        /// </summary>
        /// <param name="id">Policy id</param>
        /// <returns></returns>
        [HttpDelete("policy/{id}")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePolicy(long id)
        {
            var result = await _policyService.DeletePolicy(id);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Delete success."
            });
        }
    }
}
