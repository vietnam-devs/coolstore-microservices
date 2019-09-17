using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace CloudNativeKit.Infrastructure.Data.Dapper.Core
{
    using CloudNativeKit.Domain;

    public static class GenericRepositoryExtensions
    {
        public static async Task<TEntity> GetByIdAsync<TEntity, TId>(this IQueryRepository<TEntity, TId> repo, TId id)
            where TEntity : class, IAggregateRoot<TId>
        {
            var genericRepo = repo as GenericRepository<TEntity, TId>;
            if(genericRepo == null)
            {
                throw new System.Exception("Make sure your IQueryRepository<TEntity, TId> is a GenericRepository<TEntity, TId> instance.");
            }

            using var conn = genericRepo.SqlConnectionFactory.GetOpenConnection();
            var entities = await conn.GetListAsync<TEntity>(new { id });
            return entities.FirstOrDefault();
        }

        public static async Task<IReadOnlyCollection<TEntity>> GetByConditionAsync<TEntity, TId>(this IQueryRepository<TEntity, TId> repo, object whereConditions)
            where TEntity : class, IAggregateRoot<TId>
        {
            var genericRepo = repo as GenericRepository<TEntity, TId>;
            if (genericRepo == null)
            {
                throw new System.Exception("Make sure your IQueryRepository<TEntity, TId> is a GenericRepository<TEntity, TId> instance.");
            }

            using var conn = genericRepo.SqlConnectionFactory.GetOpenConnection();
            var entities = await conn.GetListAsync<TEntity>(whereConditions);
            return entities.ToList();
        }
    }
}
