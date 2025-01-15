namespace ANF.Core.Exceptions
{
    public class DuplicatedException : Exception
    {
        public DuplicatedException(string message) : base(message)
        {
            
        }

        public DuplicatedException(string message, Exception ex) : base(message, ex)
        {
            
        }
    }
}
