using Common.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddHttpContext(this IServiceCollection services)
        {
            if (services.All(x => x.ServiceType != typeof(IHttpContextAccessor)))
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            return services;
        }

        public static IServiceCollection AddRolePermissionAuthorize(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, RoleHandler>();

            return services;
        }
    }
}
