using System;
using System.Linq;

namespace CloudNativeKit.Domain
{
    public interface IQueryRepositoryFactory
    {
        IQueryRepositoryWithId<TEntity, TId> QueryRepository<TEntity, TId>() where TEntity : class, IAggregateRootWithId<TId>;
        IQueryRepository<TEntity> QueryRepository<TEntity>() where TEntity : class, IAggregateRoot;
    }

    public interface IQueryRepository<TEntity> : IQueryRepositoryWithId<TEntity, Guid> where TEntity : IAggregateRoot
    {
    }

    public interface IQueryRepositoryWithId<TEntity, TId> where TEntity : IAggregateRootWithId<TId>
    {
        IQueryable<TEntity> Queryable();
    }
}
