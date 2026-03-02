using Common;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IReadOnlyRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
        IQueryable<TEntity> Query(bool isTrackingEntities = false);

        PagingResult<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null!,
            PagingOption filterOptions = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!, bool asTracking = false);

        List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate = null!,
            PagingOption filterOptions = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!, bool asTracking = false);

        Task<PagingResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null!,
            PagingOption filterOptions = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!, bool asTracking = false);

        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null!,
            PagingOption filterOptions = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!, bool asTracking = false);

        TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!, bool asTracking = false);
        Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!, bool asTracking = false);

        Task<TEntity?> GetByIdAsync(string id, bool asTracking = false);
        TEntity? GetById(string id, bool asTracking = false);

        bool Any(Expression<Func<TEntity, bool>> predicate = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!);

        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        TResult? Max<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector);
        Task<TResult?> MaxAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector);
    }
}
