using Business.Interface;
using Common.Models.Commands;
using Repository.Interface;

namespace Business
{
    public class OrderBusiness(IUserRepository userReposity) : IOrderBusiness
    {
        private readonly IUserRepository _userRepository = userReposity;

        public Task<string> ConfirmOrder(ConfirmOrderRequest request)
        {
            throw new NotImplementedException();
        }

    }
}
