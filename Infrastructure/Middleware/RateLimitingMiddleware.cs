using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Middleware;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly int _limit;
    private readonly TimeSpan _period;

    public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache, IConfiguration config)
    {
        _next = next;
        _cache = cache;
        _limit = int.Parse(config.GetSection("RateLimiting")["Requests"]!);
        _period = TimeSpan.FromSeconds(int.Parse(config.GetSection("RateLimiting")["PeriodSeconds"]!));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var key = $"rl:{context.Connection.RemoteIpAddress}";
        var counter = _cache.GetOrCreate(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _period;
            return 0;
        });

        if (counter >= _limit)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
            return;
        }

        _cache.Set(
            key,
            counter + 1,
            new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = _period }
        );

        await _next(context);
    }
}