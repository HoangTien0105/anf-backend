﻿using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Core.Services
{
    public interface ITransactionService
    {
        Task<PaginationResponse<WithdrawalResponse>> GetWithdrawalRequests(int pageNumber, int pageSize, 
            string fromDate, 
            string toDate);

        Task<string> CreateDeposit(DepositRequest request);

        Task<string> CreatePaymentLinkForSubscription(SubscriptionPurchaseRequest request);

        Task<string> ConfirmPayment(long transactionId);

        Task<string> ConfirmSubscriptionPurchase(long transactionId);

        Task<string> CancelTransaction(long transactionId);

        Task<bool> CreateWithdrawalRequest(WithdrawalRequest request);

        Task<bool> UpdateWithdrawalStatus(List<long> tIds, string status);

        Task<IActionResult> ExportBatchPaymentData(List<ExportedBatchDataResponse> data);

        Task<PaginationResponse<ExportedBatchDataResponse>> GetBatchPaymentDataForExporting(PaginationRequest request, 
            string fromDate, 
            string toDate);

        Task<PaginationResponse<UserTransactionResponse>> GetTransactionOfUser(string userCode, 
            int pageNumber,
            int pageSize);

        Task<decimal> GetCurrentBalanceInWallet(string userCode);

        Task<PaginationResponse<WithdrawalResponse>> GetWithdrawalRequestsByUser(string userCode, 
            int pageNumber, 
            int pageSize);

        /// <summary>
        /// Get transaction by id
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        Task<TransactionResponse?> GetTransactionById(long transactionId);
    }
}
