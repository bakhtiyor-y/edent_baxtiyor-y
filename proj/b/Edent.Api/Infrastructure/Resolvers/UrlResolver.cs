using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Edent.Api.Infrastructure.Resolvers
{
    public class UrlResolver : IValueResolver<object, object, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UrlResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(object source, object destination, string member, ResolutionContext context)
        {
            var host = _httpContextAccessor.HttpContext.Request.Host.Value;
            if (host.Equals("localhost:4200"))
            {
                host = "localhost:5500";
            }
            return $"http://{host}";
        }
    }
}
