namespace PaymentProcessingService.Infrastructure.Events
{
    public class PaymentProcessedEvent
    {
        public bool IsApproved { get; set; }
    }
}
