namespace ANF.Core.Enums
{
    /// <summary>
    /// Validation status for tracking validation table
    /// </summary>
    public enum ValidationStatus
    {
        Unknown = 0,    // Default for tracking from offers with CPA/CPS model
        Success = 1,
        Failed = 2,
    }
}
