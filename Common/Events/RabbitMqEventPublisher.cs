using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Common.Events
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IConnection _connection;

        public RabbitMqEventPublisher(IConnection connection)
        {
            _connection = connection;
        }

        public void Publish(string queueName, IEvent message, Guid correlationId)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            using var channel = _connection.CreateModel();
            IBasicProperties props = channel.CreateBasicProperties();
            props.ContentType = "application/json";
            props.DeliveryMode = 2;
            props.MessageId = Guid.NewGuid().ToString(); 
            props.CorrelationId = correlationId.ToString(); 
            channel.QueueDeclare(queueName, true, false, false);
            channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: props,
                                 body: body);
        }
    }
}
