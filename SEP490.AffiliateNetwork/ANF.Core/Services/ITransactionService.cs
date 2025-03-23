using ANF.Core.Models.Requests;

namespace ANF.Core.Services
{
    public interface ITransactionService
    {
        Task<string> CreateDeposit(DepositRequest request);

        Task<string> ConfirmPayment(long transactionId);

        Task<string> CancelTransaction(long transactionId);
    }
}
