using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace BancaAPI.API.Filters
{
    public class AntiSpamFilter : IActionFilter
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _limiteTiempo = TimeSpan.FromSeconds(5);

        public AntiSpamFilter(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var key = $"AntiSpam_{context.HttpContext.Connection.RemoteIpAddress}";
            if (_cache.TryGetValue(key, out _))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 429,
                    Content = "Demasiadas peticiones. Intenta nuevamente en unos segundos."
                };
                return;
            }

            _cache.Set(key, true, _limiteTiempo);
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}