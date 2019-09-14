using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CloudNativeKit.Domain;
using Dapper;
using Humanizer;

namespace CloudNativeKit.Infrastructure.DataPersistence.Dapper.Query
{
    public class QueryRepositoryFactory : IQueryRepositoryFactory
    {
        private ConcurrentDictionary<Type, object> _repositories;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public QueryRepositoryFactory(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public IQueryRepository<TEntity, TId> QueryRepository<TEntity, TId>() where TEntity : class, IAggregateRoot<TId>
        {
            if (_repositories == null)
                _repositories = new ConcurrentDictionary<Type, object>();

            if (!_repositories.ContainsKey(typeof(TEntity)))
            {
                var cachedRepo = new QueryRepository<TEntity, TId>(_sqlConnectionFactory);
                _repositories[typeof(TEntity)] = cachedRepo;
            }

            return (IGenericQueryRepository<TEntity, TId>)_repositories[typeof(TEntity)];
        }
    }

    public class QueryRepository<TEntity, TId> : IGenericQueryRepository<TEntity, TId>
        where TEntity : class, IAggregateRoot<TId>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private IDbConnection _connection;
        protected IDbConnection Connection => _connection ?? (_connection = _sqlConnectionFactory.GetOpenConnection());

        public QueryRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public IQueryable<TEntity> Queryable()
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            var table = typeof(TEntity).Name.Pluralize();
            var sql = $"SELECT * FROM {table} WHERE Id=@id";
            var entities = await Connection.QueryAsync<TEntity>(sql, new { id });
            return entities.FirstOrDefault();
        }

        public async Task<IReadOnlyCollection<TEntity>> GetByConditionAsync(object whereConditions)
        {
            var entities = await Connection.GetListAsync<TEntity>(whereConditions);
            return entities.ToList();
        }

        public async Task<TEntity> InsertAsync(TEntity value)
        {
            var newId = await Connection.InsertAsync(value);
            if (newId.HasValue == false)
            {
                throw new Exception("Could not insert record to the database.");
            }

            if (value is IAggregateRoot<long> returnValue)
            {
                //todo: consider how to do it
                //returnValue.Id = newId.Value;
                return (TEntity)returnValue;
            }
            return value;
        }

        public async Task<int> UpdateAsync(TEntity value)
        {
            var numberRecordAffected = await Connection.UpdateAsync(value);
            return numberRecordAffected;
        }

        public async Task<int> DeleteById(object id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                var numberRecordAffected = await Connection.DeleteAsync(entity);
                return numberRecordAffected;
            }

            return 0;
        }
    }
}
