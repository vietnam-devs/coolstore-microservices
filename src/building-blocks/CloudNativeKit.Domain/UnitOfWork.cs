using System;
using System.Threading;
using System.Threading.Tasks;

namespace CloudNativeKit.Domain
{
    public interface IUnitOfWorkAsync : IRepositoryFactoryAsync, IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

    public interface IRepositoryFactoryAsync
    {
        IRepositoryWithIdAsync<TEntity, TId> RepositoryAsync<TEntity, TId>() where TEntity : class, IAggregateRootWithId<TId>;
        IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class, IAggregateRoot;
    }

    public interface IRepositoryAsync<TEntity> : IRepositoryWithIdAsync<TEntity, Guid> where TEntity : IAggregateRoot
    {
    }

    public interface IRepositoryWithIdAsync<TEntity, TId> where TEntity : IAggregateRootWithId<TId>
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> DeleteAsync(TEntity entity);
    }
}
