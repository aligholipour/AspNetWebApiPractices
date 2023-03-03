using AspNetWebApiPractices.Helpers;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace AspNetWebApiPractices.Extensions
{
    public static class RateLimiterExtensions
    {
        public static void AddCustomRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 10,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));

                options.RejectionStatusCode = 429;

                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;

                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                        await context.HttpContext.Response.WriteAsync($"Too many requests. Please try again after {retryAfter.TotalMinutes} minute(s).", cancellationToken: token);
                    else
                        await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken: token);

                    context.HttpContext.RequestServices.GetService<ILoggerFactory>()
                        ?.CreateLogger("RateLimitingMiddleware")
                        .LogWarning($"OnRejected: {context.HttpContext.Request.Path}");
                };

                options.AddPolicy<string, PermissionRateLimiterPolicy>("UserAuthenticated");

                options.AddFixedWindowLimiter("Api", configOptions =>
                {
                    configOptions.AutoReplenishment = true;
                    configOptions.PermitLimit = 10;
                    configOptions.Window = TimeSpan.FromMinutes(1);
                });

                options.AddFixedWindowLimiter("Web", configOptions =>
                {
                    configOptions.AutoReplenishment = true;
                    configOptions.PermitLimit = 10;
                    configOptions.Window = TimeSpan.FromMinutes(1);
                });
            });
        }
    }
}
