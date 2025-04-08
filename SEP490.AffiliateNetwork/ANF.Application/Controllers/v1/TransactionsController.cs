using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ANF.Core.Models.Entities;
using ANF.Infrastructure;
using ANF.Core.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using ANF.Core.Services;
using ANF.Core.Commons;
using ANF.Core.Models.Responses;

namespace ANF.Application.Controllers.v1
{
    public class TransactionsController : BaseApiController
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Get withdrawal requests with pagination and optional date filters
        /// </summary>
        /// <param name="request">Pagination request containing page number and page size</param>
        /// <param name="fromDate">Optional start date for filtering requests</param>
        /// <param name="toDate">Optional end date for filtering requests</param>
        /// <returns>Paginated list of withdrawal requests</returns>
        [HttpGet("users/withdrawal-requests")]
        [Authorize(Roles = "Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWithdrawalRequests([FromQuery] PaginationRequest request,
            [FromQuery] string fromDate,
            [FromQuery] string toDate)
        {
            var response = await _transactionService.GetWithdrawalRequests(request.pageNumber, request.pageSize, fromDate, toDate);
            return Ok(new ApiResponse<PaginationResponse<WithdrawalResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = response
            });
        }

        /// <summary>
        /// Deposit money from users
        /// </summary>
        /// <param name="request">Money amount</param>
        /// <returns></returns>
        [HttpPost("users/deposit")]
        [Authorize]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deposit(DepositRequest request)
        {
            var url = await _transactionService.CreateDeposit(request);
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = url
            });
        }

        /// <summary>
        /// Create withdrawal request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("users/withdrawal-request")]
        [Authorize(Roles = "Publisher, Advertiser")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateWithdrawalRequest(WithdrawalRequest request)
        {
            var result = await _transactionService.CreateWithdrawalRequest(request);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = @"Request is created successfully! Please wait for admin to approve the request, 
                            normally it takes a week to have the money back to the account 
                            due to collecting requests from other users."
            });
        }

        /// <summary>
        /// Update withdrawal status batch (for Admin)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("users/withdrawal-status")]
        [Authorize(Roles = "Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateWithdrawalRequest([FromBody] UpdatedWithdrawalRequest request)
        {
            var result = await _transactionService.UpdateWithdrawalStatus(request.TransactionIds, request.Status);
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Success."
            });
        }

        /// <summary>
        /// Revoke transaction
        /// </summary>
        /// <param name="transactionId">Transaction's id</param>
        /// <returns></returns>
        [HttpDelete("users/revoke-payment")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelPayment(long transactionId)
        {
            var url = await _transactionService.CancelTransaction(transactionId);
            return Redirect(url);
        }

        /// <summary>
        /// Confirm payment from users
        /// </summary>
        /// <param name="transactionId">Transaction's id</param>
        /// <returns></returns>
        [HttpPatch("users/confirm-payment")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmPayment(long transactionId)
        {
            var url = await _transactionService.ConfirmPayment(transactionId);
            return Redirect(url);
        }

        /// <summary>
        /// Get batch payment data for exporting
        /// </summary>
        /// <param name="request">Pagination request</param>
        /// <param name="fromDate">From date</param>
        /// <param name="toDate">To date</param>
        /// <returns></returns>
        [HttpGet("batch-payment-data")]
        //[Authorize(Roles = "Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBatchPaymentData([FromQuery] PaginationRequest request,
            [FromQuery] string fromDate,
            [FromQuery] string toDate)
        {
            var response = await _transactionService.GetBatchPaymentDataForExporting(request.pageNumber, 
                request.pageSize, 
                fromDate, 
                toDate);
            return Ok(new ApiResponse<PaginationResponse<ExportedBatchDataResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = response
            });
        }

        /// <summary>
        /// Export batch payment data
        /// </summary>
        /// <param name="data">List of exported batch data</param>
        /// <returns>BatchPaymentData.xlsx</returns>
        [HttpPost("export-batch-payment-data")]
        //[Authorize(Roles = "Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExportBatchPaymentData([FromBody] List<ExportedBatchDataResponse> data)
        {
            return await _transactionService.ExportBatchPaymentData(data);
        }

        /// <summary>
        /// Get transaction of user
        /// </summary>
        /// <param name="code">User's code</param>
        /// <param name="request">Pagination request</param>
        /// <returns></returns>
        [HttpGet("users/{code}/transactions")]
        [Authorize(Roles = "Publisher, Advertiser")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactionOfUser(string code, [FromQuery] PaginationRequest request)
        {
            var response = await _transactionService.GetTransactionOfUser(code, request.pageNumber, request.pageSize);
            return Ok(new ApiResponse<PaginationResponse<UserTransactionResponse>>
            {
                IsSuccess = true,
                Message = "Success.",
                Value = response
            });
        }
    }
}
