namespace ANF.Core.Services
{
    /// <summary>
    /// Get claims of the user from token
    /// </summary>
    public interface IUserClaimsService
    {
        IDictionary<string, string> GetClaims();

        string GetClaim(string key);
    }
}
