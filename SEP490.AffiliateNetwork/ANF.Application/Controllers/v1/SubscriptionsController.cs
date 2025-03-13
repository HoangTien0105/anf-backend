using Microsoft.AspNetCore.Mvc;
using ANF.Core.Services;
using ANF.Core.Models.Requests;
using Asp.Versioning;
using ANF.Core.Commons;
using ANF.Core.Models.Responses;
using Microsoft.AspNetCore.Authorization;

namespace ANF.Application.Controllers.v1
{
    public class SubscriptionsController(ISubscriptionService subscriptionService) : BaseApiController
    {
        private readonly ISubscriptionService _subscriptionService = subscriptionService;

        /// <summary>
        /// Get all subscriptions
        /// </summary>
        /// <param name="request">Pagination request model</param>
        /// <returns></returns>
        [HttpGet("subscriptions")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSubscriptions([FromQuery] PaginationRequest request)
        {
            var subscriptions = await _subscriptionService.GetSubscriptions(request);
            return Ok(new ApiResponse<PaginationResponse<SubscriptionResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = subscriptions
            });
        }

        /// <summary>
        /// Get subscription by id
        /// </summary>
        /// <param name="id">Subscription id</param>
        /// <returns></returns>
        [HttpGet("subscriptions/{id}")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSubscription(long id)
        {
            var subscription = await _subscriptionService.GetSubscription(id);
            return Ok(new ApiResponse<SubscriptionResponse>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = subscription
            });
        }

        /// <summary>
        /// Update subscription
        /// </summary>
        /// <param name="id">Subscription id</param>
        /// <param name="request">Subscription data</param>
        /// <returns></returns>
        [HttpPut("subscriptions/{id}")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSubscription(long id, SubscriptionRequest request)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }

            var result = await _subscriptionService.UpdateSubscription(id, request);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Update successfully"
            });
        }

        /// <summary>
        /// Create new subscription
        /// </summary>
        /// <param name="request">Subscription data</param>
        /// <returns></returns>
        [HttpPost("subscriptions")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSubscription(SubscriptionRequest request)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var result = await _subscriptionService.CreateSubscription(request);
            if (!result) return BadRequest();

            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Create subscription successfully"
            });
        }

        /// <summary>
        /// Delete subscriptions
        /// </summary>
        /// <param name="id">Subscription id</param>
        /// <returns></returns>
        [HttpDelete("subscriptions/{id}")]
        [MapToApiVersion(1)]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSubscription(long id)
        {
            var result = await _subscriptionService.DeleteSubscriptions(id);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Delete success."
            });
        }
    }
}
