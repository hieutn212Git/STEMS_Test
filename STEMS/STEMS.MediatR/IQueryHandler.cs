using Common;
using MediatR;

namespace STEMS.MediatR
{
    public interface IQueryHandler<in TRequest, TResponse> : IRequestHandler<TRequest, ApiResponse<TResponse>>
    where TRequest : IQuery<TResponse>
    {
    }
}
