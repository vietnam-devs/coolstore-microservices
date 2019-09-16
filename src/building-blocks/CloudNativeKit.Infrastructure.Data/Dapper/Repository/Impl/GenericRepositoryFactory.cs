using System;
using System.Collections.Concurrent;
using CloudNativeKit.Domain;

namespace CloudNativeKit.Infrastructure.Data.Dapper.Repository.Impl
{
    public class GenericRepositoryFactory : IQueryRepositoryFactory
    {
        private ConcurrentDictionary<Type, object> _repositories = null;
        private readonly ISqlConnectionFactory _sqlConnectionFactory = null;

        public GenericRepositoryFactory(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        // TODO: consider the name of QueryRepository later
        public IQueryRepository<TEntity, TId> QueryRepository<TEntity, TId>() where TEntity : class, IAggregateRoot<TId>
        {
            if (_repositories == null)
                _repositories = new ConcurrentDictionary<Type, object>();

            if (!_repositories.ContainsKey(typeof(TEntity)))
            {
                var cachedRepo = new GenericRepository<TEntity, TId>(_sqlConnectionFactory);
                _repositories[typeof(TEntity)] = cachedRepo;
            }

            return (IGenericRepository<TEntity, TId>)_repositories[typeof(TEntity)];
        }
    }
}
