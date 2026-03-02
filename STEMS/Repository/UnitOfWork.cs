using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repository.Interface;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public DbSet<TEntity> Get<TEntity>() where TEntity : BaseEntity
        {
            return _context.Set<TEntity>();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                _context.Dispose();
                _transaction?.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IDbContextTransaction BeginTransaction()
        {
            _transaction ??= _context.Database.BeginTransaction();
            return _transaction;
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
        }

        public void RollBackTransaction()
        {
            _transaction.Rollback();
        }
    }
}
