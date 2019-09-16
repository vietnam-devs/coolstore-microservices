using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudNativeKit.Domain;

namespace CloudNativeKit.Infrastructure.Data.Dapper.Repository
{
    public interface IGenericRepository<TEntity, TId> : IRepositoryAsync<TEntity, TId>, IQueryRepository<TEntity, TId>
        where TEntity : class, IAggregateRoot<TId>
    {
        Task<IQueryable<TEntity>> QueryableAsync();
        Task<TEntity> GetByIdAsync(TId id);
        Task<IReadOnlyCollection<TEntity>> GetByConditionAsync(object whereConditions);
    }
}
