namespace ANF.Core.Exceptions
{
    public class NoDataRetrievalException : Exception
    {
        public NoDataRetrievalException(string message) : base(message)
        {
            
        }

        public NoDataRetrievalException(string message, Exception ex) : base(message, ex) 
        {
            
        }
    }
}
