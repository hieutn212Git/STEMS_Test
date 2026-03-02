namespace Common
{
    public static class Constants
    {
    }

    public static class AuthConstants
    {
        public const string AUTHORIZATION_HEADER = "Authorization";
        public const string AUTH_SCHEME_BASIC = "BasicAuthentication";
        public const string AUTH_SCHEME_BEARER = "Bearer";
        public const string AUTH_SCHEME_COOKIES = "Cookies";

        public const string AUTHENTICATION_SCHEMES = "BasicAuthentication,Bearer";
        public const string SCHEMES_BEARER_COOKIES = "Bearer,Cookies";
    }

    public static class CustomClaimTypes
    {
        public const string UserId = "userId";
        public const string UserType = "userType";
        public const string Roles = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        public const string RoleIds = "roleIds";
    }
}
