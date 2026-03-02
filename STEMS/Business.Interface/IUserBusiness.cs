using Common.Models.Queries.Requests;
using Common.Models.Queries.Responses;

namespace Business.Interface
{
    public partial interface IUserBusiness
    {
        Task<LoginResponse> Login(LoginRequest request);
        //Task<LoginResponse> RefreshToken(RefreshTokenRequest request);
    }
}
