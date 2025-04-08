using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Responses
{
    public class UserTransactionResponse
    {
        public long Id { get; set; }

        public decimal Amount { get; set; }

        public long? CampaignId { get; set; }

        public long? SubscriptionId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
