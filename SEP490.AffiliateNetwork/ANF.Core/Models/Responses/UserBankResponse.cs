namespace ANF.Core.Models.Responses
{
    public class UserBankResponse
    {
        /// <summary>
        /// User's bank id
        /// </summary>
        public int Id { get; init; }

        public string? BankingNo { get; init; }

        public string BankingProvider { get; init; } = null!;
    }
}
