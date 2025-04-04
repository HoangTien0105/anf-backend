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
            _exchange = options.Value.Exchange;
            _serviceProvider = serviceProvider;
            _logger = logger;

            _channel.ExchangeDeclareAsync(_exchange, ExchangeType.Direct, true).GetAwaiter().GetResult();

            // Declare queue and bind to exchange
            // Các queue sẽ tương ứng cho các pricing model khác nhau
            DeclareQueue("cpc").GetAwaiter().GetResult();
            DeclareQueue("cpa").GetAwaiter().GetResult();
            DeclareQueue("cps").GetAwaiter().GetResult();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RabbitMQConsumer started at: {Time}", DateTime.Now);

            stoppingToken.Register(() => _logger.LogInformation("RabbitMQConsumer is stopping."));

            try
            {
                StartListening("cpc");
                StartListening("cpa");
                StartListening("cps");

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RabbitMQConsumer execution");
                throw;
            }
        }


        private async Task DeclareQueue(string queueName)
        {
            _logger.LogInformation("Declaring queue: {QueueName}", queueName);
            await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            await _channel.QueueBindAsync(queueName, _exchange, queueName);
            _logger.LogInformation("Queue {QueueName} declared and bound to exchange {Exchange}", queueName, _exchange);
        }

        private void StartListening(string queueName)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("[Queue: {QueueName}] Received message: {Message}", queueName, message);

                try
                {
                    var eventData = JsonConvert.DeserializeObject<TrackingConversionEvent>(message);
                    _logger.LogInformation("[Queue: {QueueName}] Deserialized event for click_id: {ClickId}", queueName, eventData.ClickId);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var processingService = scope.ServiceProvider.GetRequiredService<ITrackingService>();
                        await processingService.ProcessTrackingEvent(eventData);
                    }

                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                    _logger.LogInformation("[Queue: {QueueName}] Successfully processed and acknowledged event for click_id: {ClickId}", queueName, eventData.ClickId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[Queue: {QueueName}] Error processing message: {Message}", queueName, message);
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, false); // Không requeue, gửi đến dead-letter queue nếu có
                }
            };

            _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
            _logger.LogInformation("Started listening on queue {QueueName}", queueName);
        }
        public override void Dispose()
        {
            try
            {
                _channel?.CloseAsync().GetAwaiter().GetResult();
                _connection?.CloseAsync().GetAwaiter().GetResult();
                _logger.LogInformation("RabbitMQConsumer disposed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disposing RabbitMQConsumer");
            }
            base.Dispose();
        }
    }
}