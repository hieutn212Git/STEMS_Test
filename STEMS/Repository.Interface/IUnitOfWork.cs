using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
        void CommitTransaction();
        public void RollBackTransaction();
        DbSet<TEntity> Get<TEntity>() where TEntity : BaseEntity;
    }
}
