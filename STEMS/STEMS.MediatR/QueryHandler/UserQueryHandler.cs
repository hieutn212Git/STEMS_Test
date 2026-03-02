using Business.Interface;
using Common;
using Common.Models.Queries.Requests;
using Common.Models.Queries.Responses;

namespace STEMS.MediatR.QueryHandler
{
    public class UserQueryHandler : IQueryHandler<LoginRequest, LoginResponse>
    {
        private readonly IUserBusiness _userBusiness;

        public UserQueryHandler(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        public async Task<ApiResponse<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var response = await _userBusiness.Login(request);
            return new ApiResponse<LoginResponse>(response);
        }
    }
}
