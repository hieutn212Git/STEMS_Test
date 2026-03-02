using Common.Models.Queries.Responses;

namespace Common.Models.Queries.Requests
{
    public class LoginRequest : BaseQuery<LoginResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
