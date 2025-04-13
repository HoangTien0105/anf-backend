namespace ANF.Core.Exceptions
{
    public class InactiveWalletException : Exception
    {
        public InactiveWalletException(string message) : base(message)
        {
        }
        
        public InactiveWalletException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
