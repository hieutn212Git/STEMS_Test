using Microsoft.AspNetCore.Authorization;

namespace Common.Providers
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public IEnumerable<string> Roles { get; }

        public RoleRequirement(string roles)
        {
            Roles = (roles ?? "").Split(',');
        }
    }
}
