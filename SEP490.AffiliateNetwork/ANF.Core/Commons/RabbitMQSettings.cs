namespace ANF.Core.Commons
{
    public sealed class RabbitMQSettings
    {
        public string Host { get; init; } = null!;

        public int Port { get; init; }
        
        public string Username { get; init; } = null!;

        public string Password { get; init; } = null!;

        public string Exchange { get; init; } = null!;
    }
}
