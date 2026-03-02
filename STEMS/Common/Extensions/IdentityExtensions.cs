using System.Security.Claims;

namespace Common.Extensions
{
    public static class IdentityExtensions
    {
        public static string UserId(this ClaimsIdentity claimsIdentity)
        {
            return GetClaimValue(claimsIdentity, CustomClaimTypes.UserId);
        }

        private static string GetClaimValue(ClaimsIdentity claimsIdentity, string claimType)
        {
            Claim? claim = claimsIdentity.FindFirst(claimType);
            if (claim != null)
            {
                return claim.Value;
            }

            return string.Empty;
        }
    }
}
