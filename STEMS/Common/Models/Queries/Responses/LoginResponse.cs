namespace Common.Models.Queries.Responses
{
    public class LoginResponse
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public long? AccessTokenExpiry { get; set; }
        public string RefreshToken { get; set; }
        public long? RefreshTokenExpiry { get; set; }
    }
}
