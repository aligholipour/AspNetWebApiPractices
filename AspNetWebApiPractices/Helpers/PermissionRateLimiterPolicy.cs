using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace AspNetWebApiPractices.Helpers
{
    public class PermissionRateLimiterPolicy : IRateLimiterPolicy<string>
    {
        public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected { get; } = (context, _) =>
        {
            context.HttpContext.Response.StatusCode = 418;
            return new ValueTask();
        };

        public RateLimitPartition<string> GetPartition(HttpContext httpContext)
        {
            if (httpContext.User.Identity?.IsAuthenticated is true) 
            {
                return RateLimitPartition.GetFixedWindowLimiter(httpContext.User.Identity.Name!,
                    partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 500,
                        Window = TimeSpan.FromMinutes(1),
                    });
            }

            return RateLimitPartition.GetFixedWindowLimiter(httpContext.Request.Headers.Host.ToString(),
                    partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1),
                    });
        }
    }
}
