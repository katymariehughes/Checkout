namespace Gateway.API.Services
{
    public interface IDateTimeOracle
    {
        DateTime Now { get; }
    }

    public class DateTimeOracle : IDateTimeOracle
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
