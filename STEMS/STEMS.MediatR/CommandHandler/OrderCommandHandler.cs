using Common;
using Common.Models.Commands;

namespace STEMS.MediatR.CommandHandler
{
    public partial class OrderCommandHandler : ICommandHandler<ConfirmOrderRequest>
    {
        public async Task<ApiResponse> Handle(ConfirmOrderRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
