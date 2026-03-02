using DataAccess.Context;
using DataAccess.Entities;
using Repository.Interface;

namespace Repository
{
    public class UserRepository(CoreDataContext context) : GenericRepository<User>(context), IUserRepository
    {
    }
}
