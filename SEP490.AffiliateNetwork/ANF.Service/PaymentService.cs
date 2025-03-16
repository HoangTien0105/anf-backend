using ANF.Core;
using ANF.Core.Commons;
using ANF.Core.Models.Entities;
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

        public Task<bool> CancelPaymentLink()
        {
            throw new NotImplementedException();
        }

        public async Task<string> CreatePaymentLink()
        {
            try
            {
                var transactionRepository = _unitOfWork.GetRepository<Core.Models.Entities.Transaction>();
                var userRepository = _unitOfWork.GetRepository<User>();

                PayOS payOS = new PayOS(_options.PayOSClientId, _options.PayOSApiKey, _options.PayOSChecksumKey);
                ItemData data = new ItemData("Nạp tiền vào hệ thống ANF", 1, 1000); //TODO: Change the amount of money based on the requested data
                List<ItemData> items = new List<ItemData>();
                items.Add(data);

                PaymentData paymentData = new PaymentData(orderCode: 1, amount: 2, description: "Nạp tiền cho xxx", 
                    items, cancelUrl: string.Empty, returnUrl: string.Empty);   //TODO: Change the value of these parameters
                CreatePaymentResult result = await payOS.createPaymentLink(paymentData);
                return result.checkoutUrl;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            throw new NotImplementedException();
        }

        public Task<string> GetPaymentLinkInformation()
        {
            throw new NotImplementedException();
        }
    }
}
