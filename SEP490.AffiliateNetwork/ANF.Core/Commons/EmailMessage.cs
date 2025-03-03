namespace ANF.Core.Commons
{
    public class EmailMessage
    {
        public string To { get; set; } = null!;
        
        public string Subject { get; set; } = null!;
        
        public string? Body { get; set; }
    }
}
