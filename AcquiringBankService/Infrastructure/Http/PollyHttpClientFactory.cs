using Flurl.Http.Configuration;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;
using System.Diagnostics;

namespace AcquiringBankService.Infrastructure.Http
{
    public class PollyHttpClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new PolicyHandler
            {
                InnerHandler = base.CreateMessageHandler()
            };
        }
    }

    public class PolicyHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Policies.PolicyStrategy.ExecuteAsync(ct => base.SendAsync(request, ct), cancellationToken);
        }
    }

    public static class Policies
    {
        private static AsyncTimeoutPolicy<HttpResponseMessage> TimeoutPolicy
            => Policy.TimeoutAsync<HttpResponseMessage>(2, (context, timeSpan, task) =>
            {
                Debug.WriteLine($"[App|Policy]: Timeout delegate fired after {timeSpan.Seconds} seconds");
                return Task.CompletedTask;
            });

        private static AsyncRetryPolicy<HttpResponseMessage> RetryPolicy
            => Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                     .Or<TimeoutRejectedException>()
                     .WaitAndRetryAsync(new[]
                     {
                         TimeSpan.FromSeconds(1),
                         TimeSpan.FromSeconds(2),
                         TimeSpan.FromSeconds(5)
                     },
                     (delegateResult, retryCount) =>
                     {
                         Debug.WriteLine($"[App|Policy]: Retry delegate fired, attempt {retryCount}");
                     });

        public static AsyncPolicyWrap<HttpResponseMessage> PolicyStrategy
            => Policy.WrapAsync(RetryPolicy, TimeoutPolicy);
    }
}
