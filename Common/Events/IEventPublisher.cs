namespace Common.Events
{
    public interface IEventPublisher
    {
        void Publish(string queueName, IEvent message, Guid correlationId);
    }
}
