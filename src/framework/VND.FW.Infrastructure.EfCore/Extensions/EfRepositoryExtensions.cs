using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VND.Fw.Domain;

namespace VND.FW.Infrastructure.EfCore.Extensions
{
    public static class EfRepositoryExtensions
    {
        public static async Task<TEntity> GetByIdAsync<TDbContext, TEntity>(
            this IEfQueryRepository<TDbContext, TEntity> repo, 
            Guid id, 
            params Expression<Func<TEntity, object>>[] includeProperties)
                where TDbContext : DbContext
                where TEntity : class, IEntity

        {
            var queryable = repo.Queryable().AsNoTracking() as IQueryable<TEntity>;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    queryable = queryable.Include(includeProperty);
                }
            }

            return await queryable.SingleOrDefaultAsync(e => e.Id.Equals(id));
        }

        public static async Task<IReadOnlyList<TEntity>> ListAsync<TDbContext, TEntity>(
            this IEfQueryRepository<TDbContext, TEntity> repo,
            params Expression<Func<TEntity, object>>[] includeProperties)
                where TDbContext : DbContext
                where TEntity : class, IEntity
        {
            var queryable = repo.Queryable().AsNoTracking() as IQueryable<TEntity>;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    queryable = queryable.Include(includeProperty);
                }
            }

            return await queryable.ToListAsync();
        }

        internal static async Task<PaginatedItem<TResponse>> QueryAsync<TDbContext, TEntity, TResponse>(
            this IEfQueryRepository<TDbContext, TEntity> repo,
            Criterion criterion,
            Expression<Func<TEntity, TResponse>> selector,
            params Expression<Func<TEntity, object>>[] includeProperties)
                where TDbContext : DbContext
                where TEntity : class, IEntity
        {
            return await GetDataAsync(repo, criterion, selector, null, includeProperties);
        }

        internal static async Task<PaginatedItem<TResponse>> FindAllAsync<TDbContext, TEntity, TResponse>(
            this IEfQueryRepository<TDbContext, TEntity> repo,
            Criterion criterion,
            Expression<Func<TEntity, TResponse>> selector,
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] includeProperties)
                where TDbContext : DbContext
                where TEntity : class, IEntity
        {
            return await GetDataAsync(repo, criterion, selector, filter, includeProperties);
        }

        internal static async Task<TEntity> FindOneAsync<TDbContext, TEntity>(
            this IEfQueryRepository<TDbContext, TEntity> repo,
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] includeProperties)
                where TDbContext : DbContext
                where TEntity : class, IEntity
        {
            var dbSet = repo.Queryable();
            foreach (var includeProperty in includeProperties)
            {
                dbSet = dbSet.Include(includeProperty);
            }

            return await dbSet.FirstOrDefaultAsync(filter);
        }

        private static async Task<PaginatedItem<TResponse>> GetDataAsync<TDbContext, TEntity, TResponse>(
            IEfQueryRepository<TDbContext, TEntity> repo,
            Criterion criterion,
            Expression<Func<TEntity, TResponse>> selector,
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
                where TDbContext : DbContext
                where TEntity : class, IEntity
        {
            if (criterion.PageSize < 1 || criterion.PageSize > criterion.DefaultPagingOption.PageSize)
            {
                criterion.SetPageSize(criterion.DefaultPagingOption.PageSize);
            }

            var queryable = repo.Queryable();
            if (includeProperties != null && includeProperties.Count() > 0)
            {
                queryable = includeProperties.Aggregate(
                    queryable,
                    (current, include) => current.Include(include));
            }

            if (filter != null)
                queryable = queryable.Where(filter);

            if (!string.IsNullOrWhiteSpace(criterion.SortBy))
            {
                var isDesc = string.Equals(criterion.SortOrder, "desc", StringComparison.OrdinalIgnoreCase) ? true : false;
                queryable = queryable.OrderByPropertyName(criterion.SortBy, isDesc);
            }

            var results = await queryable
                .Skip(criterion.CurrentPage * criterion.PageSize)
                .Take(criterion.PageSize)
                .AsNoTracking()
                .Select(selector)
                .ToListAsync();

            var totalRecord = await queryable.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecord / criterion.PageSize);

            if (criterion.CurrentPage > totalPages)
            {
                criterion.SetCurrentPage(totalPages);
            }

            return new PaginatedItem<TResponse>(totalRecord, totalPages, results);
        }
    }
}
