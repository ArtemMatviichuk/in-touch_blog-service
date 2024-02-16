
using System.Text;
using BlogService.Common.Constants;
using BlogService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BlogService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection? _connection;
        private IModel? _chanel;
        private string? _queueName;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;

            InitializeRabbitMQ();
        }

        public override void Dispose()
        {
            if (_chanel != null && _chanel.IsOpen)
            {
                _chanel.Close();
                _connection?.Close();
            }

            base.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_chanel);
            consumer.Received += async (moduleHandle, ea) =>
                await _eventProcessor.ProcessEvent(Encoding.UTF8.GetString(ea.Body.ToArray()));

            _chanel?.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private void InitializeRabbitMQ()
        {
            Console.WriteLine($"--> RabbitMQ address: {_configuration[AppConstants.RabbitMQHost]}:{_configuration[AppConstants.RabbitMQPort]}");
            var factory = new ConnectionFactory()
            {
                HostName = _configuration[AppConstants.RabbitMQHost],
                Port = int.Parse(_configuration[AppConstants.RabbitMQPort]!)
            };

            _connection = factory.CreateConnection();
            _chanel = _connection.CreateModel();
            _chanel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _queueName = _chanel.QueueDeclare().QueueName;
            _chanel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");

            Console.WriteLine("--> Listening on the Message Bus");

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection Shutdown");
        }
    }
}