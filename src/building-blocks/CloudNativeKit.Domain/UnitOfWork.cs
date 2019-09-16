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
        IRepositoryAsync<TEntity, TId> RepositoryAsync<TEntity, TId>() where TEntity : class, IAggregateRoot<TId>;
    }

    public interface IRepositoryAsync<TEntity, TId> where TEntity : IAggregateRoot<TId>
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
    }
}
