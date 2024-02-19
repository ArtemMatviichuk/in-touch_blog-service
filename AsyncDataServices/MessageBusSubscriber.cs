using System.Text;
using BlogService.AppSettingsOptions;
using BlogService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BlogService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly RabbitMQOptions _options;
        private readonly IEventProcessor _eventProcessor;
        private readonly ILogger<MessageBusSubscriber> _logger;

        private IConnection? _connection;
        private IModel? _chanel;
        private string? _consumerTag;

        public MessageBusSubscriber(RabbitMQOptions options, IEventProcessor eventProcessor,
            ILogger<MessageBusSubscriber> logger)
        {
            _options = options;
            _eventProcessor = eventProcessor;
            _logger = logger;

            InitializeRabbitMQ();
        }

        public override void Dispose()
        {
            if (_chanel != null && _chanel.IsOpen)
            {
                _chanel.BasicCancel(_consumerTag);

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
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    try
                    {
                        await _eventProcessor.ProcessEvent(message);
                        _chanel?.BasicAck(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"RabbitMQ message was not processed properly: {ex.Message}");
                        throw;
                    }
                };

            _consumerTag = _chanel?.BasicConsume(_options.QueueName, false, consumer: consumer);

            return Task.CompletedTask;
        }

        private void InitializeRabbitMQ()
        {
            Console.WriteLine($"--> RabbitMQ address: {_options.Host}:{_options.Host}");
            var factory = new ConnectionFactory()
            {
                HostName = _options.Host,
                Port = int.Parse(_options.Port),
                ClientProvidedName = _options.ClientProvidedName,
                UserName = _options.UserName,
                Password = _options.Password,
            };

            _connection = factory.CreateConnection();
            _chanel = _connection.CreateModel();

            _chanel.ExchangeDeclare(_options.Exchange, ExchangeType.Direct);
            _chanel.QueueDeclare(_options.QueueName, false, false, false, null);
            _chanel.QueueBind(_options.QueueName, _options.Exchange, _options.RoutingKey, null);
            _chanel.BasicQos(0, 1, false);

            Console.WriteLine("--> Listening on the Message Bus");

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection Shutdown");
        }
    }
}