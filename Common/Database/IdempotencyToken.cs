namespace Common.Database
{
    public class IdempotencyToken
    {
        public Guid MessageId { get; set; }

        public string Consumer { get; set; }
    }
}
