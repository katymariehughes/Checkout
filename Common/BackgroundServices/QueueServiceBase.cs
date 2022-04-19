using Common.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace Common.BackgroundServices
{
    public abstract class QueueServiceBase<TService, TMessage> : BackgroundService where TService : class where TMessage : IEvent
    {
        protected readonly ILogger<TService> _logger;
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;
        private readonly string _serviceName;

        public QueueServiceBase(ILogger<TService> logger, string consumingQueueName)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queueName = consumingQueueName ?? throw new ArgumentNullException(nameof(consumingQueueName));
            _serviceName = typeof(TService).Name;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _connectionFactory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@katymariehughes.local:5672/"),
                DispatchConsumersAsync = true
            };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclarePassive(_queueName);
            _channel.BasicQos(0, 1, false);
            _logger.LogInformation("{ServiceName} is waiting for messages on queue '{QueueName}'...", _serviceName, _queueName);

            return base.StartAsync(cancellationToken);
        }

        protected abstract Task HandleMessage(TMessage message, Guid messageId, Guid correlationId);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (bc, ea) =>
            {
                var messageBody = Encoding.UTF8.GetString(ea.Body.ToArray());
                try
                {
                    var message = JsonConvert.DeserializeObject<TMessage>(messageBody);
                    _logger.LogInformation("Message [{MessageId}] received on queue '{QueueName}'", ea.BasicProperties.MessageId, _queueName);
                    await HandleMessage(message, Guid.Parse(ea.BasicProperties.MessageId), Guid.Parse(ea.BasicProperties.CorrelationId));
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"JSON Parse Error: '{messageBody} {ex.Message}'.");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
                catch (AlreadyClosedException)
                {
                    _logger.LogInformation("RabbitMQ is closed!");
                }
                catch (Exception e)
                {
                    _logger.LogError("Message [{MessageId}] from queue '{QueueName}' threw an exception: {Exception}", ea.BasicProperties.MessageId, _queueName, e);
                    _logger.LogError(e, e.Message);
                }
                finally
                {
                    _logger.LogInformation("Message [{MessageId}] handled on queue '{QueueName}'", ea.BasicProperties.MessageId, _queueName);
                }
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            await Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            _connection.Close();
            _logger.LogInformation("{ServiceName} RabbitMQ connection is closed.", _serviceName);
        }
    }
}