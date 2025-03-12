namespace ANF.Core.Models.Responses
{
    public class UserBankResponse
    {
        public int BankingNo { get; init; }

        public string BankingProvider { get; init; } = null!;
    }
}
