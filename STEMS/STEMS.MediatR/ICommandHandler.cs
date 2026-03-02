using Common;
using MediatR;

namespace STEMS.MediatR
{
    public interface ICommandHandler<in TRequest> : IRequestHandler<TRequest, ApiResponse>
    where TRequest : ICommand
    {
    }
}
