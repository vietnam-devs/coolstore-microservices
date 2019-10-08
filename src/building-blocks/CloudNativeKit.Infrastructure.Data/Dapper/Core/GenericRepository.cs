using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudNativeKit.Domain;
using Dapper;
using ReflectionMagic;

namespace CloudNativeKit.Infrastructure.Data.Dapper.Core
{
    public class GenericRepository<TEntity, TId> : IRepositoryAsync<TEntity, TId>, IQueryRepository<TEntity, TId>
        where TEntity : class, IAggregateRoot<TId>
    {
        public ISqlConnectionFactory SqlConnectionFactory { get; }
        public IEnumerable<IDomainEventDispatcher> EventBuses { get; }

        public GenericRepository(ISqlConnectionFactory sqlConnectionFactory, IEnumerable<IDomainEventDispatcher> eventBuses)
        {
            SqlConnectionFactory = sqlConnectionFactory;
            EventBuses = eventBuses;
        }

        public IQueryable<TEntity> Queryable()
        {
            using var conn = SqlConnectionFactory.GetOpenConnection();
            var entities = conn.GetList<TEntity>();
            return entities.AsQueryable();
        }

        public async Task<TEntity> GetByIdAsync(TId id)
        {
            using var conn = SqlConnectionFactory.GetOpenConnection();
            var entities = await conn.GetListAsync<TEntity>(new { id });
            return entities.FirstOrDefault();
        }

        public async Task<IReadOnlyCollection<TEntity>> GetByConditionAsync(object whereConditions)
        {
            using var conn = SqlConnectionFactory.GetOpenConnection();
            var entities = await conn.GetListAsync<TEntity>(whereConditions);
            return entities.ToList();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            using var conn = SqlConnectionFactory.GetOpenConnection();
            var newId = await conn.InsertAsync<TId, TEntity>(entity);
            if (entity is IAggregateRoot<TId> returnValue)
            {
                returnValue.AsDynamic().Id = newId;
                return (TEntity)returnValue;
            }

            await DispatchEvents(entity);
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            using var conn = SqlConnectionFactory.GetOpenConnection();
            var numberRecordAffected = await conn.UpdateAsync(entity);
            if (numberRecordAffected <= 0)
            {
                throw new Exception("Could not update record to the database.");
            }

            await DispatchEvents(entity);
            return await GetByIdAsync(entity.Id);
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            using var conn = SqlConnectionFactory.GetOpenConnection();
            var numberRecordAffected = await conn.DeleteAsync(entity);

            if (numberRecordAffected <= 0)
            {
                throw new Exception("Could not delete record in the database.");
            }

            await DispatchEvents(entity);
            return numberRecordAffected;
        }

        private async Task DispatchEvents(TEntity entity)
        {
            foreach (var @event in entity.GetUncommittedEvents())
            {
                foreach (var eventBus in EventBuses)
                {
                    await eventBus.Dispatch(@event);
                }
            }

            entity.ClearUncommittedEvents();
        }
    }
}
