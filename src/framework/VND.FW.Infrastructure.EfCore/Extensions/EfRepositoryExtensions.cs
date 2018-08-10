using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using VND.Fw.Domain;
using VND.Fw.Infrastructure.EfCore.Repository;

namespace VND.Fw.Infrastructure.EfCore.Extensions
{
  public static class EfRepositoryExtensions
  {
    public static async Task<TEntity> GetByIdAsync<TDbContext, TEntity>(
        this IEfQueryRepository<TDbContext, TEntity> repo,
        Guid id,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool tracking = true)
            where TDbContext : DbContext
            where TEntity : class, IEntity

    {
      var queryable = repo.Queryable();

      if (!tracking)
      {
        queryable = queryable.AsNoTracking();
      }

      if (include != null)
      {
        queryable = include(queryable);
      }

      return await queryable.SingleOrDefaultAsync(e => e.Id.Equals(id));
    }

    public static async Task<TEntity> FindOneAsync<TDbContext, TEntity>(
        this IEfQueryRepository<TDbContext, TEntity> repo,
        Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true)
            where TDbContext : DbContext
            where TEntity : class, IEntity
    {
      var queryable = repo.Queryable();

      include?.Invoke(queryable);

      if (disableTracking)
      {
        queryable = queryable.AsNoTracking() as IQueryable<TEntity>;
      }

      return await queryable.FirstOrDefaultAsync(filter);
    }

    public static async Task<IReadOnlyList<TEntity>> ListAsync<TDbContext, TEntity>(
        this IEfQueryRepository<TDbContext, TEntity> repo,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true)
            where TDbContext : DbContext
            where TEntity : class, IEntity
    {
      var queryable = repo.Queryable();
      if (disableTracking)
      {
        queryable = queryable.AsNoTracking() as IQueryable<TEntity>;
      }

      include?.Invoke(queryable);

      return await queryable.ToListAsync();
    }

    public static async Task<PaginatedItem<TResponse>> QueryAsync<TDbContext, TEntity, TResponse>(
        this IEfQueryRepository<TDbContext, TEntity> repo,
        Criterion criterion,
        Expression<Func<TEntity, TResponse>> selector,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true)
            where TDbContext : DbContext
            where TEntity : class, IEntity
    {
      return await GetDataAsync(repo, criterion, selector, null, include, disableTracking);
    }

    public static async Task<PaginatedItem<TResponse>> FindAllAsync<TDbContext, TEntity, TResponse>(
        this IEfQueryRepository<TDbContext, TEntity> repo,
        Criterion criterion,
        Expression<Func<TEntity, TResponse>> selector,
        Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true)
            where TDbContext : DbContext
            where TEntity : class, IEntity
    {
      return await GetDataAsync(repo, criterion, selector, filter, include, disableTracking);
    }

    private static async Task<PaginatedItem<TResponse>> GetDataAsync<TDbContext, TEntity, TResponse>(
        IEfQueryRepository<TDbContext, TEntity> repo,
        Criterion criterion,
        Expression<Func<TEntity, TResponse>> selector,
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true)
            where TDbContext : DbContext
            where TEntity : class, IEntity
    {
      var queryable = repo.Queryable();
      if (disableTracking)
      {
        queryable = queryable.AsNoTracking() as IQueryable<TEntity>;
      }

      include?.Invoke(queryable);

      if (filter != null)
      {
        queryable = queryable.Where(filter);
      }

      if (!string.IsNullOrWhiteSpace(criterion.SortBy))
      {
        bool isDesc = string.Equals(criterion.SortOrder, "desc", StringComparison.OrdinalIgnoreCase) ? true : false;
        queryable = queryable.OrderByPropertyName(criterion.SortBy, isDesc);
      }

      var results = await queryable
        .Skip(criterion.CurrentPage * criterion.PageSize)
        .Take(criterion.PageSize)
        .Select(selector)
        .ToListAsync();

      var totalRecord = await queryable.CountAsync();
      var totalPages = (int)Math.Ceiling((double)totalRecord / criterion.PageSize);

      if (criterion.CurrentPage > totalPages)
      {
        // criterion.SetCurrentPage(totalPages);
      }

      return new PaginatedItem<TResponse>(totalRecord, totalPages, results);
    }
  }
}
