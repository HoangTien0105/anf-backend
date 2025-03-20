namespace ANF.Core.Commons
{
    /// <summary>
    /// Claims' name after decoding Bearer token
    /// </summary>
    public class ClaimConstants
    {
        public const string Primarysid = "primarysid";

        public const string NameId = "nameid";

        public const string Email = "email";
        
        /// <summary>
        /// Not valid before
        /// </summary>
        public const string Nbf = "nbf";
        
        /// <summary>
        /// Expiration time
        /// </summary>
        public const string Exp = "exp";
        
        /// <summary>
        /// Issued at
        /// </summary>
        public const string Iat = "iat";
        
        /// <summary>
        /// Issuer (who created and signed this token)
        /// </summary>
        public const string Iss = "iss";

        /// <summary>
        /// Audience (who or what the token is intended for)
        /// </summary>
        public const string Aud = "aud";
    }
}
