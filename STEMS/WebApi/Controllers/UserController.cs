using Asp.Versioning;
using Common;
using Common.Models.Queries.Requests;
using Common.Models.Queries.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    public class UserController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("login")]
        [ApiVersion("1.0")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), 200)]
        [AllowAnonymous]
        public async Task<ApiResponse> Login([FromBody] LoginRequest request)
        {
            return await _mediator.Send<ApiResponse>(request);
        }
    }
}
