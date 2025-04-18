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
        
        private readonly string _appBaseUrLDev = "http://localhost:5272/api/affiliate-network";
        
        private readonly string _appBaseUrLProd = " https://be.l3on.id.vn/api/affiliate-network";

        private readonly string _currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? string.Empty;

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

            if (_currentEnv == "Development")
            {
                var paymentData = new PaymentData(
                    orderCode: transaction.Id,
                    amount: (int)transaction.Amount,
                    description: transaction.Reason ?? string.Empty,
                    items: items,
                    cancelUrl: _appBaseUrLDev + $"/users/revoke-payment?transactionId={transaction.Id}",
                    returnUrl: _appBaseUrLDev + $"/users/confirm-payment?transactionId={transaction.Id}"
                );
                var paymentResult = await payOS.createPaymentLink(paymentData);
                return paymentResult.checkoutUrl;

            }
            else
            {
                var paymentData = new PaymentData(
                    orderCode: transaction.Id,
                    amount: (int)transaction.Amount,
                    description: transaction.Reason ?? string.Empty,
                    items: items,
                    cancelUrl: _appBaseUrLProd + $"/users/revoke-payment?transactionId={transaction.Id}",
                    returnUrl: _appBaseUrLProd + $"/users/confirm-payment?transactionId={transaction.Id}"
                );
                var paymentResult = await payOS.createPaymentLink(paymentData);
                return paymentResult.checkoutUrl;
            }
        }

        public async Task<string> CreatePaymentLinkForSubscription(long transactionId)
        {
            var transactionRepository = _unitOfWork.GetRepository<Core.Models.Entities.Transaction>();
            PayOS payOS = new PayOS(_options.ClientId, _options.ApiKey, _options.ChecksumKey);
            List<ItemData> items = new List<ItemData>();

            var transaction = await transactionRepository.FindByIdAsync(transactionId);
            if (transaction is null)
                throw new KeyNotFoundException("Transaction does not exist!");
            ItemData item = new ItemData(transaction.Reason ?? string.Empty, 1, (int)transaction.Amount);
            items.Add(item);

            if (_currentEnv == "Development")
            {
                var paymentData = new PaymentData(
                    orderCode: transaction.Id,
                    amount: (int)transaction.Amount,
                    description: transaction.Reason ?? string.Empty,
                    items: items,
                    cancelUrl: _appBaseUrLDev + $"/users/revoke-payment?transactionId={transaction.Id}",
                    returnUrl: _appBaseUrLDev + $"/users/confirm-subscription-purchase?transactionId={transaction.Id}"
                );
                var paymentResult = await payOS.createPaymentLink(paymentData);
                return paymentResult.checkoutUrl;
            }
            else
            {
                var paymentData = new PaymentData(
                    orderCode: transaction.Id,
                    amount: (int)transaction.Amount,
                    description: transaction.Reason ?? string.Empty,
                    items: items,
                    cancelUrl: _appBaseUrLProd + $"/users/revoke-payment?transactionId={transaction.Id}",
                    returnUrl: _appBaseUrLProd + $"/users/confirm-subscription-purchase?transactionId={transaction.Id}"
                );
                var paymentResult = await payOS.createPaymentLink(paymentData);
                return paymentResult.checkoutUrl;
            }
        }

        public Task<string> GetPaymentLinkInformation()
        {
            throw new NotImplementedException();
        }
    }
}
