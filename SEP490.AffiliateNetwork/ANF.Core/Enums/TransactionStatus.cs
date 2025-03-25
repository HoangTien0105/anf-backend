namespace ANF.Core.Enums
{
    public enum TransactionStatus
    {
        Success = 1,    // Using for creating campaign, buying subscription, deposit
        Failed = 2, // Using for creating campaign, buying subscription, deposit
        Canceled = 3,   // Using for creating campaign, buying subscription, deposit
        Pending = 4,
        Approved = 5,   // For withdrawing the money from users
        Rejected = 6,   // For withdrawing the money from users
    }
}
