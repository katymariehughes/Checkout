using DotNetCore.CAP;

namespace Common.Events
{
    public static class CapExtensions
    {
        public static void Publish<T>(this ICapPublisher capPublisher, string name, T contentObj, Guid correlationId)
        {
            capPublisher.Publish(name, contentObj, new Dictionary<string, string> {
                { "correlation-id", correlationId.ToString() },
                { "message-id", Guid.NewGuid().ToString() }
            });
        }

        public static (Guid messageId, Guid correlationId) GetIds(this CapHeader header) 
            => (Guid.Parse(header["message-id"]), Guid.Parse(header["correlation-id"]));
    }
}
