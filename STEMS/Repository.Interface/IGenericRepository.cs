using DataAccess.Entities;

namespace Repository.Interface
{
    public interface IGenericRepository<TEntity> : IReadOnlyRepository<TEntity>, IEditableRepository<TEntity> where TEntity : BaseEntity
    {
    }
}
