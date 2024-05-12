using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Webion.Stargaze.Auth.Services.Jwt.Exchange;

internal sealed class ExchangeCodeManager : IExchangeCodeManager
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<ExchangeCodeManager> _logger;

    public ExchangeCodeManager(IMemoryCache cache, ILogger<ExchangeCodeManager> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    
    public ValueTask<Guid> GetCodeAsync(TokenPair tokens)
    {
        var code = Guid.NewGuid();
        var key = CacheKey(code);

        _logger.LogInformation("Set exchange code {Code}", code);
        
        _cache.Set(key, tokens, TimeSpan.FromSeconds(5));
        return ValueTask.FromResult(code);
    }

    public ValueTask<TokenPair?> ExchangeCodeAsync(Guid code)
    {
        var key = CacheKey(code);
        var found = _cache.TryGetValue(key, out TokenPair? pair);

        _logger.LogInformation("Exchange code {Code} found: {Found}", code, found);
        
        return ValueTask.FromResult(pair);
    }

    private string CacheKey(Guid code) => $"connect:exchange:{code}";
}