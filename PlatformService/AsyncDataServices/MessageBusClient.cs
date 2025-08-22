using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata;
using PlatformService.Dtos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _config;
        private readonly SemaphoreSlim _connectionSemaphore = new(1, 1);
        private readonly string _exchange = "trigger";
        private IConnection? _connection;
        private IChannel? _channel;
        private bool _isConnected = false;
        public MessageBusClient(IConfiguration configuration)
        {
            _config = configuration;
        }

        private async Task EnsureConnectedAsync()
        {
            if (_isConnected && _connection != null && _channel != null)
            {
                return;
            }

            await _connectionSemaphore.WaitAsync();
            try
            {
                if (_isConnected && _connection != null && _channel != null)
                {
                    return;
                }

                Console.WriteLine("Establishing connection...");
                await ConnectAsync();
                _isConnected = true;
            }
            finally
            {
                _connectionSemaphore.Release();
            }
        }

        public async Task ConnectAsync()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config.GetValue<string>("RabbitMQHost", "localhost")!,
                Port = _config.GetValue<int>("RabbitMQPort", 5672),
                ClientProvidedName = "Platforms_Service"
            };

            try
            {
                _connection = await factory.CreateConnectionAsync("");
                Console.WriteLine("--> Connected to RabbitMQ");
                _channel = await _connection.CreateChannelAsync();

                await _channel.ExchangeDeclareAsync(exchange: _exchange, type: ExchangeType.Fanout);

                Console.WriteLine("Connected to message bus!");

                _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdownAsync;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the message bus: {ex.Message}");
                throw;
            }
        }

        public async Task PublishNewPlatformAsync(PlatformPublishedDto platformPublishedDto)
        {
            await EnsureConnectedAsync();
            if (_connection == null || _channel == null)
            {
                throw new InvalidOperationException("RabbitMQ connection not established.");
            };

            var message = JsonSerializer.Serialize(platformPublishedDto);

            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ connection open, sending message...");
                await SendMessage(message);
            }
            else {
                Console.WriteLine("--> RabbitMQ Connection is closed, not sending");
            }
        }

        private async Task SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            var props = new BasicProperties();
            await _channel.BasicPublishAsync(
                exchange: _exchange,
                routingKey: "",
                mandatory: false,
                basicProperties: props,
                body: body,
                cancellationToken: default
            );

            Console.WriteLine($"--> We have sent {message}");
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
                await _connection.DisposeAsync();
        }

        private async Task RabbitMQ_ConnectionShutdownAsync(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"RabbitMQ connection lost. Reason: {e.ReplyText}, ReplyCode: {e.ReplyCode}");

            _isConnected = false;
            _connection = null;
            _channel = null;
            _connectionSemaphore.Dispose();
        }
    }
}
