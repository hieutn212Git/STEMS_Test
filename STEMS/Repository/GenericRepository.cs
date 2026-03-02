using Common;
using Common.Extensions;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repository.Interface;
using System.Linq.Expressions;

namespace Repository
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>()!;
        }

        public IQueryable<TEntity> Query(bool isTrackingEntities = false)
        {
            if (isTrackingEntities)
            {
                return _dbSet.AsTracking().Where(w => !w.Deleted);
            }
            return _dbSet.AsNoTracking().Where(w => !w.Deleted);
        }

        public virtual TEntity InsertOrUpdate(TEntity entity, bool saveChange = false)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                Insert(entity, false);
            }
            else
            {
                Update(entity, false);
            }

            if (saveChange)
            {
                _context.SaveChanges();
            }

            return entity;
        }

        public int InsertOrUpdate(IEnumerable<TEntity> entities, bool saveChange = false)
        {
            var result = 0;
            foreach (var entity in entities)
            {
                if (string.IsNullOrEmpty(entity.Id))
                {
                    Insert(entity, false);
                }
                else
                {
                    Update(entity, false);
                }
            }

            if (saveChange)
            {
                result = _context.SaveChanges();
            }
            return result;
        }

        public async Task<TEntity> InsertOrUpdateAsync(TEntity entity, bool saveChange = false)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                await InsertAsync(entity, false);
            }
            else
            {
                await UpdateAsync(entity, false);
            }

            if (saveChange)
            {
                await _context.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<int> InsertOrUpdateAsync(IEnumerable<TEntity> entities, bool saveChange = false)
        {
            var result = 0;
            foreach (var entity in entities)
            {
                if (string.IsNullOrEmpty(entity.Id))
                {
                    await InsertAsync(entity, false);
                }
                else
                {
                    await UpdateAsync(entity, false);
                }
            }

            if (saveChange)
            {
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public PagingResult<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null!,
            PagingOption filterOptions = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!, bool asTracking = false)
        {
            IQueryable<TEntity> query = Query(asTracking);
            var result = new PagingResult<TEntity>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include(query);
            }

            result.TotalRecords = query.Count(w => !w.Deleted);

            if (filterOptions != null)
            {
                foreach (var item in filterOptions.Sorter)
                {
                    if (item.Value == "descend")
                    {
                        query = query.OrderByDescending(item.Key.CapitalizeFirstLetter());
                    }
                    else if (item.Value == "ascend")
                    {
                        query = query.OrderBy(item.Key.CapitalizeFirstLetter());
                    }
                }

                filterOptions.PageSize = filterOptions.PageSize <= 0 ? 10 : filterOptions.PageSize;
                filterOptions.PageIndex = filterOptions.PageIndex <= 0 ? 1 : filterOptions.PageIndex;

                query = query.Skip((filterOptions.PageIndex - 1) * filterOptions.PageSize).Take(filterOptions.PageSize);

                result.PageSize = filterOptions.PageSize;
                result.PageIndex = filterOptions.PageIndex;
            }

            result.Data = query.ToList();
            return result;
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate = null!,
            PagingOption filterOptions = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!, bool asTracking = false)
        {
            IQueryable<TEntity> query = Query(asTracking);
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (filterOptions != null)
            {
                foreach (var item in filterOptions.Sorter)
                {
                    if (item.Value == "descend")
                    {
                        query = query.OrderByDescending(item.Key.CapitalizeFirstLetter());
                    }
                    else if (item.Value == "ascend")
                    {
                        query = query.OrderBy(item.Key.CapitalizeFirstLetter());
                    }
                }

                filterOptions.PageSize = filterOptions.PageSize <= 0 ? 10 : filterOptions.PageSize;
                filterOptions.PageIndex = filterOptions.PageIndex <= 0 ? 1 : filterOptions.PageIndex;

                query = query.Skip((filterOptions.PageIndex - 1) * filterOptions.PageSize).Take(filterOptions.PageSize);
            }

            return [.. query];
        }

        public async Task<PagingResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null!,
            PagingOption filterOptions = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!, bool asTracking = false)
        {
            IQueryable<TEntity> query = Query(asTracking);
            var result = new PagingResult<TEntity>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include(query);
            }

            result.TotalRecords = query.Count(w => !w.Deleted);

            if (filterOptions != null)
            {
                foreach (var item in filterOptions.Sorter)
                {
                    if (item.Value == "descend")
                    {
                        query = query.OrderByDescending(item.Key.CapitalizeFirstLetter());
                    }
                    else if (item.Value == "ascend")
                    {
                        query = query.OrderBy(item.Key.CapitalizeFirstLetter());
                    }
                }

                filterOptions.PageSize = filterOptions.PageSize <= 0 ? 10 : filterOptions.PageSize;
                filterOptions.PageIndex = filterOptions.PageIndex <= 0 ? 1 : filterOptions.PageIndex;

                query = query.Skip((filterOptions.PageIndex - 1) * filterOptions.PageSize).Take(filterOptions.PageSize);

                result.PageSize = filterOptions.PageSize;
                result.PageIndex = filterOptions.PageIndex;
            }

            result.Data = await query.ToListAsync();
            return result;
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null!,
            PagingOption filterOptions = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!, bool asTracking = false)
        {
            IQueryable<TEntity> query = Query(asTracking);
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (filterOptions != null)
            {
                foreach (var item in filterOptions.Sorter)
                {
                    if (item.Value == "descend")
                    {
                        query = query.OrderByDescending(item.Key.CapitalizeFirstLetter());
                    }
                    else if (item.Value == "ascend")
                    {
                        query = query.OrderBy(item.Key.CapitalizeFirstLetter());
                    }
                }

                filterOptions.PageSize = filterOptions.PageSize <= 0 ? 10 : filterOptions.PageSize;
                filterOptions.PageIndex = filterOptions.PageIndex <= 0 ? 1 : filterOptions.PageIndex;

                query = query.Skip((filterOptions.PageIndex - 1) * filterOptions.PageSize).Take(filterOptions.PageSize);
            }

            return await query.ToListAsync();
        }

        public TEntity? GetById(string id, bool asTracking = false)
        {
            if (asTracking)
            {
                return _dbSet.AsTracking().FirstOrDefault(w => !w.Deleted && w.Id == id);
            }
            else
            {
                return _dbSet.AsTracking().FirstOrDefault(w => !w.Deleted && w.Id == id);
            }
        }

        public async Task<TEntity?> GetByIdAsync(string id, bool asTracking = false)
        {
            if (asTracking)
            {
                return await _dbSet.AsTracking().FirstOrDefaultAsync(w => !w.Deleted && w.Id == id);
            }
            else
            {
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(w => !w.Deleted && w.Id == id);
            }
        }

        public TResult? Max<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector)
        {
            return Query().Where(predicate)
                .Select(selector)
                .DefaultIfEmpty()
                .Max();
        }

        public async Task<TResult?> MaxAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector)
        {
            return await Query().Where(predicate)
                            .Select(selector)
                            .DefaultIfEmpty()
                            .MaxAsync();
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!, bool asTracking = false)
        {
            IQueryable<TEntity> query;
            if (!asTracking)
            {
                query = _dbSet.AsNoTracking().Where(w => !w.Deleted);
            }
            else
            {
                query = _dbSet.AsTracking().Where(w => !w.Deleted);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include(query);
            }

            return query.FirstOrDefault();
        }

        public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!, bool asTracking = false)
        {
            IQueryable<TEntity> query;
            if (!asTracking)
            {
                query = _dbSet.AsNoTracking().Where(w => !w.Deleted);
            }
            else
            {
                query = _dbSet.AsTracking().Where(w => !w.Deleted);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual TEntity Insert(TEntity entity, bool saveChange = false)
        {
            entity.CreatedDateTime = DateTime.UtcNow;
            entity.UpdatedDateTime = DateTime.UtcNow;
            entity.RowNumber = 1;
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
            }
            SetCreatedByForEntity(entity);
            _dbSet.Add(entity);
            if (saveChange)
            {
                _context.SaveChanges();
            }
            return entity;
        }

        public int Insert(IEnumerable<TEntity> entities, bool saveChange = false)
        {
            var result = 0;
            foreach (var entity in entities)
            {
                Insert(entity, false);
            }

            if (saveChange)
            {
                result = _context.SaveChanges();
            }
            return result;
        }

        public async Task<TEntity> InsertAsync(TEntity entity, bool saveChange = false)
        {
            entity.CreatedDateTime = DateTime.UtcNow;
            entity.UpdatedDateTime = DateTime.UtcNow;
            entity.RowNumber = 1;
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
            }
            SetCreatedByForEntity(entity);
            await _dbSet.AddAsync(entity);
            if (saveChange)
            {
                await _context.SaveChangesAsync();
            }
            return entity;
        }

        public async Task<int> InsertAsync(IEnumerable<TEntity> entities, bool saveChange = false)
        {
            var result = 0;
            foreach (var entity in entities)
            {
                await InsertAsync(entity, false);
            }

            if (saveChange)
            {
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public TEntity Update(TEntity entity, bool saveChange = false)
        {
            var entityId = entity.Id;
            if (!_dbSet.Any(w => !w.Deleted && w.Id == entityId)) { return entity; }
            entity.UpdatedDateTime = DateTime.UtcNow;
            entity.RowNumber++;
            SetUpdatedByForEntity(entity);
            _context.Update(entity);
            if (saveChange)
            {
                _context.SaveChanges();
            }
            return entity;
        }

        public int Update(IEnumerable<TEntity> entities, bool saveChange = false)
        {
            var result = 0;
            foreach (var entity in entities)
            {
                Update(entity, false);
            }

            if (saveChange)
            {
                result = _context.SaveChanges();
            }
            return result;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, bool saveChange = false)
        {
            var entityId = entity.Id;
            if (!_dbSet.Any(w => !w.Deleted && w.Id == entityId)) { return entity; }
            entity.UpdatedDateTime = DateTime.UtcNow;
            entity.RowNumber++;
            SetUpdatedByForEntity(entity);
            _context.Update(entity);
            if (saveChange)
            {
                await _context.SaveChangesAsync();
            }
            return entity;
        }

        public async Task<int> UpdateAsync(IEnumerable<TEntity> entities, bool saveChange = false)
        {
            var result = 0;
            foreach (var entity in entities)
            {
                await UpdateAsync(entity, false);
            }

            if (saveChange)
            {
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public string Delete(TEntity entity, bool saveChange = false)
        {
            var entityId = entity.Id;
            if (!_dbSet.Any(w => w.Id == entityId)) { return entityId; }
            _dbSet.Remove(entity);
            if (saveChange)
            {
                _context.SaveChanges();
            }
            return entityId;
        }

        public int Delete(Expression<Func<TEntity, bool>> predicate, bool saveChange = false)
        {
            var entities = Query().Where(predicate).ToList();
            return Delete(entities, saveChange);
        }

        public int Delete(IEnumerable<TEntity> entities, bool saveChange = false)
        {
            int result = 0;
            _dbSet.RemoveRange(entities);
            if (saveChange)
            {
                result = _context.SaveChanges();
            }
            return result;
        }

        public async Task<string> DeleteAsync(TEntity entity, bool saveChange = false)
        {
            var entityId = entity.Id;
            if (!_dbSet.Any(w => w.Id == entityId)) { return entityId; }
            _dbSet.Remove(entity);
            if (saveChange)
            {
                await _context.SaveChangesAsync();
            }
            return entityId;
        }

        public async Task<int> DeleteAsync(IEnumerable<TEntity> entities, bool saveChange = false)
        {
            int result = 0;
            if (entities.Any())
            {
                _dbSet.RemoveRange(entities);
                if (saveChange)
                {
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }

        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool saveChange = false)
        {
            var entities = Query().Where(predicate).ToList();
            return await DeleteAsync(entities, saveChange);
        }

        public string SoftDelete(TEntity entity, bool saveChange = false)
        {
            var entityId = entity.Id;
            if (!_dbSet.Any(w => !w.Deleted && w.Id == entityId)) { return entityId; }

            entity.UpdatedDateTime = DateTime.UtcNow;
            entity.Deleted = true;
            SetUpdatedByForEntity(entity);
            _context.Update(entity);
            if (saveChange)
            {
                _context.SaveChanges();
            }
            return entity.Id;
        }

        public int SoftDelete(IEnumerable<TEntity> entities, bool saveChange = false)
        {
            int result = 0;

            foreach (var entity in entities)
            {
                SoftDelete(entity, false);
            }

            if (saveChange)
            {
                result = _context.SaveChanges();
            }
            return result;
        }

        public int SoftDelete(Expression<Func<TEntity, bool>> predicate, bool saveChange = false)
        {
            var entities = Query().Where(predicate).ToList();
            return SoftDelete(entities, saveChange);
        }

        public async Task<string> SoftDeleteAsync(TEntity entity, bool saveChange = false)
        {
            var entityId = entity.Id;
            if (!_dbSet.Any(w => !w.Deleted && w.Id == entityId)) { return entity.Id; }

            entity.UpdatedDateTime = DateTime.UtcNow;
            entity.Deleted = true;
            SetUpdatedByForEntity(entity);
            _context.Update(entity);
            if (saveChange)
            {
                await _context.SaveChangesAsync();
            }
            return entity.Id;
        }

        public async Task<int> SoftDeleteAsync(IEnumerable<TEntity> entities, bool saveChange = false)
        {
            int result = 0;

            foreach (var entity in entities)
            {
                await SoftDeleteAsync(entity, false);
            }

            if (saveChange)
            {
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> SoftDeleteAsync(Expression<Func<TEntity, bool>> predicate, bool saveChange = false)
        {
            var entities = Query().Where(predicate).ToList();
            return await SoftDeleteAsync(entities, saveChange);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!)
        {
            IQueryable<TEntity> query = Query(false);

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include(query);
            }

            return query.Any();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!)
        {
            IQueryable<TEntity> query = Query(false);

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include(query);
            }

            return await query.AnyAsync();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = Query().Where(predicate);
            return query.Count();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = Query().Where(predicate);
            return await query.CountAsync();
        }

        #region Private

        protected static void SetCreatedByForEntity(TEntity entity)
        {
            if (string.IsNullOrEmpty(entity.CreatedBy))
            {
                var currentUserId = HttpContextExtensions.Identity?.UserId()!;
                entity.CreatedBy = currentUserId;
                entity.LastUpdatedBy = currentUserId;
            }
        }

        private static void SetUpdatedByForEntity(TEntity entity)
        {
            if (string.IsNullOrEmpty(entity.LastUpdatedBy))
            {
                var currentUserId = HttpContextExtensions.Identity?.UserId()!;
                entity.LastUpdatedBy = currentUserId;
            }
        }
        #endregion
    }
}
