using DotNetCore.CAP.Filter;
using Microsoft.Extensions.Logging;

namespace Common.Events
{
    public class CapFilter : SubscribeFilter
    {
        private readonly ILogger<CapFilter> _logger;

        public CapFilter(ILogger<CapFilter> logger)
        {
            _logger = logger;
        }

        public override void OnSubscribeExecuting(ExecutingContext context)
        {
            var headers = context.DeliverMessage.Headers;
            _logger.LogDebug("{MessageType} [{MessageId}] received on queue '{QueueName}'", headers["cap-msg-type"], headers["message-id"], headers["cap-msg-name"]);
        }

        public override void OnSubscribeExecuted(ExecutedContext context)
        {
            var headers = context.DeliverMessage.Headers;
            _logger.LogDebug("{MessageType} [{MessageId}] handled", headers["cap-msg-type"], headers["message-id"]);
        }
    }
}
