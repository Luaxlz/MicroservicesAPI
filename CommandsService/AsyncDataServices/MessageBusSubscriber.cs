using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            await InitializeRabbitMQ();

            if (_channel == null || _queueName == null)
            {
                throw new InvalidOperationException("RabbitMQ channel or queue name is not initialized.");
            }


            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Event Received!");
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                _eventProcessor.ProcessEvent(notificationMessage);
                return Task.CompletedTask;
            };

            await _channel.BasicConsumeAsync(
                queue: _queueName,
                autoAck: true,
                consumer: consumer,
                cancellationToken: stoppingToken
            );

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

        private Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine("--> Event Received!");

            var body = e.Body.ToArray();
            var notificationMessage = Encoding.UTF8.GetString(body);

            try
            {
                _eventProcessor.ProcessEvent(notificationMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not process event: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        private Task RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
            return Task.CompletedTask;
        }

        public override async void Dispose()
        {
            if (_channel?.IsOpen == true)
            {
                await _channel.CloseAsync();
            }
            
            if (_connection?.IsOpen == true)
            {
                await _connection.CloseAsync();
            }

            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}