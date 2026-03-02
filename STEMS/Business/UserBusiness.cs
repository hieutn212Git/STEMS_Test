using Business.Interface;
using Common.Models.Queries.Requests;
using Common.Models.Queries.Responses;
using Repository.Interface;

namespace Business
{
    public class UserBusiness(IUserRepository userReposity) : IUserBusiness
    {
        private readonly IUserRepository _userRepository = userReposity;

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = _userRepository.GetFirstOrDefault(x => x.Username == request.Username);

            return new LoginResponse();
        }

        //public Task<LoginResponse> RefreshToken(RefreshTokenRequest request)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
