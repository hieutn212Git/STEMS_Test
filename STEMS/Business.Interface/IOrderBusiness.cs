using Common.Models.Commands;

namespace Business.Interface
{
    public interface IOrderBusiness
    {
        Task<string> ConfirmOrder(ConfirmOrderRequest request);

    }
}
