namespace ANF.Core.Models.Responses
{
    public class UserStatusResponse
    {
        public long UserId { get; init; }

        public string Status { get; init; } = null!;
    }
}
