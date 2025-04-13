namespace ANF.Core.Services
{
    /// <summary>
    /// Interface for payment service using PayOS
    /// </summary>
    public interface IPaymentService
    {
        Task<string> CreatePaymentLink(long transactionId);

        Task<string> CreatePaymentLinkForSubscription(long transactionId);

        Task<string> GetPaymentLinkInformation();

        Task<bool> CancelPaymentLink(); 
    }
}
