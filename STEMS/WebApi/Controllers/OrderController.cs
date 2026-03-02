using Asp.Versioning;
using Common;
using Common.Models.Commands;
using Common.Models.Queries.Requests;
using Common.Models.Queries.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    public class OrderController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("Confirm")]
        [ApiVersion("1.0")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public async Task<ApiResponse> Confirm([FromBody] ConfirmOrderRequest request)
        {
            return await _mediator.Send<ApiResponse>(request);
        }
    }
}
