using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CloudNativeKit.Infrastructure.Data.Dapper.Core
{
    using CloudNativeKit.Domain;

    public interface IDapperUnitOfWork : IUnitOfWork
    {
        ISqlConnectionFactory SqlConnectionFactory { get; }
    }

    public class DapperUnitOfWork : IDapperUnitOfWork
    {
        private ConcurrentDictionary<string, object> _repositories = null;
        public ISqlConnectionFactory SqlConnectionFactory { get; }

        public DapperUnitOfWork(ISqlConnectionFactory sqlConnectionFactory)
        {
            SqlConnectionFactory = sqlConnectionFactory;
        }

        public IQueryRepository<TEntity, TId> QueryRepository<TEntity, TId>() where TEntity : class, IAggregateRoot<TId>
        {
            if (_repositories == null)
                _repositories = new ConcurrentDictionary<string, object>();

            var key = $"{typeof(TEntity)}-query";
            if (!_repositories.ContainsKey(key))
            {
                var cachedRepo = new GenericRepository<TEntity, TId>(SqlConnectionFactory);
                _repositories[key] = cachedRepo;
            }

            return (IQueryRepository<TEntity, TId>)_repositories[key];
        }

        public IRepositoryAsync<TEntity, TId> RepositoryAsync<TEntity, TId>() where TEntity : class, IAggregateRoot<TId>
        {
            if (_repositories == null)
                _repositories = new ConcurrentDictionary<string, object>();

            var key = $"{typeof(TEntity)}-command";
            if (!_repositories.ContainsKey(key))
            {
                var cachedRepo = new GenericRepository<TEntity, TId>(SqlConnectionFactory);
                _repositories[key] = cachedRepo;
            }

            return (IRepositoryAsync<TEntity, TId>)_repositories[key];
        }

        public int SaveChanges()
        {
            //TODO: just for compatible with the IUnitOfWork interface
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            //TODO: just for compatible with the IUnitOfWork interface
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if(SqlConnectionFactory != null)
            {
                GC.SuppressFinalize(SqlConnectionFactory);
            }
        }
    }
}
