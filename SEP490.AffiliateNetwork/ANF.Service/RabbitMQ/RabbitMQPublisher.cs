using ANF.Core.Commons;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace ANF.Service.RabbitMQ
{
    public sealed class RabbitMQPublisher : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly string _exchange;

        public RabbitMQPublisher(IOptions<RabbitMQSettings> options)
        {
            var factory = new ConnectionFactory
            {
                HostName = options.Value.Host,
                UserName = options.Value.Username,
                Password = options.Value.Password,
                VirtualHost = options.Value.VirtualHost,
            };
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().Result;
            _exchange = options.Value.Exchange;

            _channel.ExchangeDeclareAsync(_exchange, ExchangeType.Direct, true).GetAwaiter().GetResult();
        }

        public async Task PublishAsync(string routingKey, object message)
        {
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            await _channel.BasicPublishAsync(
                exchange: _exchange,
                routingKey: routingKey,
                mandatory: false, // False để tránh exception nếu không có queue nào nhận message
                body: body
            );
        }
        
        public void Dispose()
        {
            _connection.Dispose();
            _channel.Dispose();
        }
    }
}