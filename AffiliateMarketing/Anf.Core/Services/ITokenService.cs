namespace Anf.Core.Services
{
    /// <summary>
    /// Provides methods for generating tokens.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates a new access token.
        /// </summary>
        /// <returns>A string representing the generated access token.</returns>
        string GenerateAccessToken();
    }
}
