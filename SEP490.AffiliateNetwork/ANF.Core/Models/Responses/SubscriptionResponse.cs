namespace ANF.Core.Models.Responses
{
    /// <summary>
    /// Response model for subscriptions
    /// </summary>
    public class SubscriptionResponse
    {
        public long Id { get; init; }

        public string Name { get; init; } = null!;

        public string? Description { get; init; }

        public decimal PricePerMonth { get; init; }

        public decimal PricePerYear { get; init; }

        public string? PricingBenefit { get; init; }

        public int MaxCreatedCampaign { get; init; }
    }
}
