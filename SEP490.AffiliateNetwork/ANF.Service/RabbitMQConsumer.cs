using ANF.Core.Commons;
using ANF.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ANF.Service
{
    public sealed class RabbitMQConsumer : BackgroundService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly string _exchange;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RabbitMQConsumer> _logger;

        public RabbitMQConsumer(IOptions<RabbitMQSettings> options, 
            IServiceProvider serviceProvider,
            ILogger<RabbitMQConsumer> logger)
        {
            var factory = new ConnectionFactory
            {
                HostName = options.Value.Host,
                UserName = options.Value.Username,
                Password = options.Value.Password
            };

            _connection = factory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;
            _exchange = options.Value.ExchangeName;
            _serviceProvider = serviceProvider;
            _logger = logger;

            _channel.ExchangeDeclareAsync(_exchange, ExchangeType.Direct, true).GetAwaiter().GetResult();

            // Declare queue and bind to exchange
            // Các queue sẽ tương ứng cho các pricing model khác nhau
            // CPC; CPS/CPA
            DeclareQueue("cpc").GetAwaiter().GetResult();
            DeclareQueue("cps_cpa").GetAwaiter().GetResult();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            StartListening("cpc");
            StartListening("cps_cpa");

            return Task.CompletedTask;
        }

        private async Task DeclareQueue(string queueName)
        {
            await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            await _channel.QueueBindAsync(queueName, _exchange, queueName);
        }

        private void StartListening(string queueName)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    // TODO: Tạo 1 class để hứng data nhận về từ queue
                    var eventData = JsonConvert.DeserializeObject<object>(message);
                    //_logger.LogInformation($"[Queue: {queueName}] Received event for click_id: {eventData.ClickId}");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var processingService = scope.ServiceProvider.GetRequiredService<ITrackingService>();
                        //await processingService.ProcessTrackingEvent(queueName, eventData);
                    }

                    // Xác nhận đã xử lý thành công
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error processing message: {ex.Message}");
                    // Có thể implement dead-letter queue nếu cần
                }
            };

            _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
        }
    }
}
