using ANF.Core.Commons;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using ANF.Service;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    public class PostbackController(IPostbackService postbackService) : BaseApiController
    {
        private readonly IPostbackService _postbackService = postbackService;

        /// <summary>
        /// Get postback by transaction's id
        /// </summary>
        /// <param name="id">Transaction's id</param>
        /// <returns></returns>
        [HttpGet("postbacks/{id}")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostbackDataByTransactionId(string id)
        {
            var postback = await _postbackService.GetPostbackDataByTransactionId(id);
            return Ok(new ApiResponse<PostbackData>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = postback
            });
        }

        /// <summary>
        /// Create postback
        /// </summary>
        /// <param name="request">Postback Create Request</param>
        /// <returns></returns>
        [HttpPost("postbacks")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreatePostback([FromBody] PostbackRequest request)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var result = await _postbackService.CreatePostBack(request);
            if (!result) return BadRequest();

            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Create postback successfully."
            });
        }

        /// <summary>
        /// Create postback's log
        /// </summary>
        /// <param name="request">Postback's log request</param>
        /// <returns></returns>
        [HttpPost("postbacks/log")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePostbackLog([FromBody] PurchaseLogRequest request)
        {
            var validationResult = HandleValidationErrors();
            if (validationResult is not null)
            {
                return validationResult;
            }
            var result = await _postbackService.CreatePurchaseLog(request);
            if (!result) return BadRequest();

            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Create postback log successfully."
            });
        }
        /// <summary>
        /// Update postback's status
        /// </summary>
        /// <param name="id">Postback's id</param>
        /// <param name="status">Postback's status</param>
        /// <returns></returns>
        [HttpPatch("postbacks")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePostbackStatus(long id, string status)
        {
            var result = await _postbackService.UpdatePostBackStatus(id, status);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Update success."
            });
        }

        /// <summary>
        /// Update postback's log information
        /// </summary>
        /// <param name="id">Click's id</param>
        /// <param name="request">Postback's log data</param>
        /// <returns></returns>
        [HttpPut("postbacks/log/{id}")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePostbackLog(string id, PostbackLogUpdateRequest request)
        {
            var result = await _postbackService.UpdatePostBackLog(id, request);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Update success."
            });
        }

        /// <summary>
        /// Get postback logs by click's id
        /// </summary>
        /// <param name="id">Click's Id</param>
        /// <returns></returns>
        [HttpGet("postbacks/log/{id}/click")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostbacksLogByClickId(string id)
        {
            var postbackLogs = await _postbackService.GetAllPostbackLogByClickId(id);
            return Ok(new ApiResponse<List<PurchaseLog>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = postbackLogs
            });
        }

        /// <summary>
        /// Get postback logs by transaction's id
        /// </summary>
        /// <param name="id">Transaction's Id</param>
        /// <returns></returns>
        [HttpGet("postbacks/log/{id}/transaction")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostbacksLogByTransactionId(string id)
        {
            var postbackLogs = await _postbackService.GetAllPostbackLogByTransactionId(id);
            return Ok(new ApiResponse<List<PurchaseLog>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = postbackLogs
            });
        }

        /// <summary>
        /// Get postback logs by id
        /// </summary>
        /// <param name="id">Postback logs's Id</param>
        /// <returns></returns>
        [HttpGet("postbacks/log/{id}")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostbacksLogId(long id)
        {
            var postbackLog = await _postbackService.GetPostbackLogById(id);
            return Ok(new ApiResponse<PurchaseLog>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = postbackLog
            });
        }


    }
}
