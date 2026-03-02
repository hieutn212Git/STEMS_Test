using MediatR;

namespace Common
{
    public abstract class BaseCommand : ICommand
    {
        public string? UserId { get; set; }
    }

    public interface ICommand : IRequest<ApiResponse>
    {
    }
}
