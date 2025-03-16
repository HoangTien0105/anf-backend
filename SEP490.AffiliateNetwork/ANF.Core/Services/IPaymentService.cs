namespace ANF.Core.Services
{
    /// <summary>
    /// Interface for payment service using PayOS
    /// </summary>
    public interface IPaymentService
    {
        Task<string> CreatePaymentLink();

        Task<string> GetPaymentLinkInformation();

        Task<bool> CancelPaymentLink(); 
    }
}
