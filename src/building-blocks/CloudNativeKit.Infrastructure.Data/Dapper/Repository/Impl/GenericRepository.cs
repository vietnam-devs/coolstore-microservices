using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudNativeKit.Domain;
using Dapper;
using ReflectionMagic;

namespace CloudNativeKit.Infrastructure.Data.Dapper.Repository.Impl
{
    public class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId>
        where TEntity : class, IAggregateRoot<TId>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GenericRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public IQueryable<TEntity> Queryable()
        {
            throw new Exception("Use QueryableAsync, instead of using Queryable.");
        }

        public async Task<IQueryable<TEntity>> QueryableAsync()
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection();
            var entities = await conn.GetListAsync<TEntity>();
            return entities.AsQueryable();
        }

        public async Task<TEntity> GetByIdAsync(TId id)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection();
            var entities = await conn.GetListAsync<TEntity>(new { id });
            return entities.FirstOrDefault();
        }

        public async Task<IReadOnlyCollection<TEntity>> GetByConditionAsync(object whereConditions)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection();
            var entities = await conn.GetListAsync<TEntity>(whereConditions);
            return entities.ToList();
        }

        public async Task<TEntity> AddAsync(TEntity value)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection();
            var newId = await conn.InsertAsync<TId, TEntity>(value);
            if (value is IAggregateRoot<TId> returnValue)
            {
                returnValue.AsDynamic().Id = newId;
                return (TEntity)returnValue;
            }
            return value;
        }

        public async Task<TEntity> UpdateAsync(TEntity value)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection();
            var numberRecordAffected = await conn.UpdateAsync(value);
            if (numberRecordAffected <= 0)
            {
                throw new Exception("Could not update record to the database.");
            }

            return await GetByIdAsync(value.Id);
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            using var conn = _sqlConnectionFactory.GetOpenConnection();
            var numberRecordAffected = await conn.DeleteAsync(entity);
            return numberRecordAffected;
        }
    }
}
