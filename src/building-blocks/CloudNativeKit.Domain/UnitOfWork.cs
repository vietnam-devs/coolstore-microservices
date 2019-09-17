using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CloudNativeKit.Domain
{
    public interface IUnitOfWork : IRepositoryFactory, IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

    public interface IRepositoryFactory
    {
        IQueryRepository<TEntity, TId> QueryRepository<TEntity, TId>() where TEntity : class, IAggregateRoot<TId>;
        IRepositoryAsync<TEntity, TId> RepositoryAsync<TEntity, TId>() where TEntity : class, IAggregateRoot<TId>;
    }

    public interface IQueryRepository<TEntity, TId> where TEntity : IAggregateRoot<TId>
    {
        IQueryable<TEntity> Queryable();
    }

    public interface IRepositoryAsync<TEntity, TId> where TEntity : IAggregateRoot<TId>
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
    }
}
