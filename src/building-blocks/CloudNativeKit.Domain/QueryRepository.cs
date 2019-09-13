using System.Linq;

namespace CloudNativeKit.Domain
{
    public interface IQueryRepositoryFactory
    {
        IQueryRepository<TEntity, TId> QueryRepository<TEntity, TId>() where TEntity : class, IAggregateRoot<TId>;
    }

    public interface IQueryRepository<TEntity, TId> where TEntity : IAggregateRoot<TId>
    {
        IQueryable<TEntity> Queryable();
    }
}
