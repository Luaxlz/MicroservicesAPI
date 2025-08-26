
using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandsService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection? _connection;
        private IChannel? _channel;
        private string? _queueName;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
        }

        private async Task InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"] ?? "localhost",
                Port = int.Parse(_configuration["RabbitMQPort"] ?? "5672"),
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(exchange: "trigger", type: ExchangeType.Fanout);
            _queueName = (await _channel.QueueDeclareAsync()).QueueName;

            await _channel.QueueBindAsync(
                queue: _queueName,
                exchange: "trigger",
                routingKey: string.Empty
            );

            Console.WriteLine("--> Listening on the Message Bus...");

            _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        private Task RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
            return Task.CompletedTask;
        }
        
        public override void Dispose()
        {
            if (_channel?.IsOpen == true)
            {
                _channel.CloseAsync();
                _connection?.CloseAsync();
            }

            base.Dispose();
        }
    }
}