using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Common.Extensions
{
    public static class HttpContextExtensions
    {
        private static IHttpContextAccessor _contextAccessor;

        public static HttpContext? Current => _contextAccessor?.HttpContext;
        public static ClaimsIdentity? Identity => Current?.User.Identity as ClaimsIdentity;
        public static void Configure(IHttpContextAccessor? contextAccessor)
        {
            if (contextAccessor == null)
            {
                throw new ArgumentNullException(nameof(contextAccessor));
            }

            _contextAccessor = contextAccessor;
        }
    }
}
