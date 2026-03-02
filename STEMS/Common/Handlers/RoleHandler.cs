using Common.Providers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Common.Handlers
{
    public class RoleHandler : IAuthorizationHandler
    {
        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            var result = true;
            foreach (var requirement in context.PendingRequirements)
            {
                if (requirement is RoleRequirement roleRequirement)
                {
                    if (!CheckRoles(context.User, (roleRequirement).Roles))
                    {
                        result = false;
                        break;
                    }

                    context.Succeed(requirement);
                }
            }

            if (!result)
            {
                context.Fail();
            }

            await Task.CompletedTask;
        }

        protected bool CheckPermissions(ClaimsPrincipal user, IEnumerable<string> requirePermissions)
        {
            if (user.Identity!.IsAuthenticated)
            {
                var permissions = requirePermissions?.ToList() ?? new List<string>();
                if (user.Claims.Any(c => c.Type == "Permission"
                                        && permissions.Any(p => p == c.Value)))
                {
                    return true;
                }
            }

            return false;
        }

        protected bool CheckRoles(ClaimsPrincipal user, IEnumerable<string> requireRoles)
        {
            if (user.Identity!.IsAuthenticated)
            {
                var permissions = requireRoles?.ToList() ?? new List<string>();
                if (user.Claims.Any(c => c.Type == "Role"
                                        && permissions.Any(p => p == c.Value)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
