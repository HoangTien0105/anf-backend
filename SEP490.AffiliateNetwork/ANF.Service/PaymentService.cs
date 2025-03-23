using ANF.Core;
using ANF.Core.Commons;
using ANF.Core.Services;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;

namespace ANF.Service
{
    public class PaymentService(IUnitOfWork unitOfWork, IOptions<PayOSSettings> options) : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly PayOSSettings _options = options.Value;
        private readonly string _appBaseUrl = "http://localhost:5272/api/affiliate-network";

        public Task<bool> CancelPaymentLink()
        {
            throw new NotImplementedException();
        }

        public async Task<string> CreatePaymentLink(long transactionId)
        {
            var transactionRepository = _unitOfWork.GetRepository<Core.Models.Entities.Transaction>();
            PayOS payOS = new PayOS(_options.ClientId, _options.ApiKey, _options.ChecksumKey);
            List<ItemData> items = new List<ItemData>();

            var transaction = await transactionRepository.FindByIdAsync(transactionId);
            if (transaction is null)
                throw new KeyNotFoundException("Transaction does not exist!");
            ItemData item = new ItemData(transaction.Reason ?? string.Empty, 1, (int)transaction.Amount);
            items.Add(item);
            
            //NOTE: Please change the base url based on the environment (Dev, Prod)
            var paymentData = new PaymentData(
                orderCode: transaction.Id,
                amount: (int)transaction.Amount,
                description: transaction.Reason ?? string.Empty,
                items: items,
                cancelUrl: _appBaseUrl + $"/users/revoke-payment?transactionId={transaction.Id}",
                returnUrl: _appBaseUrl + $"/users/confirm-payment?transactionId={transaction.Id}"
            );  
            var paymentResult = await payOS.createPaymentLink(paymentData);
            return paymentResult.checkoutUrl;
        }

        public Task<string> GetPaymentLinkInformation()
        {
            throw new NotImplementedException();
        }
    }
}
