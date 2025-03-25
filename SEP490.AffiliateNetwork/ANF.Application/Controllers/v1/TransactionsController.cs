using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ANF.Core.Models.Entities;
using ANF.Infrastructure;
using ANF.Core.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using ANF.Core.Services;
using ANF.Core.Commons;

namespace ANF.Application.Controllers.v1
{
    public class TransactionsController : BaseApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionService _transactionService;

        public TransactionsController(ApplicationDbContext context, ITransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetPaymentTransactions()
        {
            return await _context.PaymentTransactions.ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(long id)
        {
            var transaction = await _context.PaymentTransactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
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
        [Authorize]
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
                Message = "Request is created successfully! Please wait for admin to approve the request."
            });
        }

        [HttpPost("users/withdrawals-status")]
        [Authorize(Roles = "Admin")]
        [MapToApiVersion(1)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateWithdrawalRequest()  //FIX THE ENDPOINT LATER
        {
            var result = true;
            if (!result) return BadRequest();
            return Ok(new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Request is created successfully! Please wait for admin to approve the request."
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

    }
}
