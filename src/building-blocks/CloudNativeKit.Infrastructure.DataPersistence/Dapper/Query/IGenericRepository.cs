using System.Collections.Generic;
using System.Threading.Tasks;
using CloudNativeKit.Domain;

namespace CloudNativeKit.Infrastructure.DataPersistence.Dapper.Query
{
    public interface IGenericQueryRepository<TEntity, TId> : IQueryRepository<TEntity, TId>
        where TEntity : class, IAggregateRoot<TId>
    {
        Task<TEntity> GetByIdAsync(object id);
        Task<IReadOnlyCollection<TEntity>> GetByConditionAsync(object whereConditions);
    }

    public interface IGenericCommandRepository<TEntity, TId>
        where TEntity : class, IAggregateRoot<TId>
    {
        Task<TEntity> InsertAsync(TEntity value);
        Task<int> UpdateAsync(TEntity value);
        Task<int> DeleteById(object id);
    }
}
