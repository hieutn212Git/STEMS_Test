using MediatR;

namespace Common
{
    public abstract class BaseQuery<TResponse> : IQuery<TResponse>
    {
        public string? UserId { get; set; }
    }

    public abstract class BaseListQuery<TResponse> : IQuery<TResponse>
    {
        public PagingOption FilterOptions { get; set; } = new PagingOption();
    }

    public interface IQuery<TResponse> : IRequest<ApiResponse<TResponse>>
    {
    }
}
