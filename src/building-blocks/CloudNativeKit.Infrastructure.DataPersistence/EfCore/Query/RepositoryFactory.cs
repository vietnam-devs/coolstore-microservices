using System;
using System.Collections.Concurrent;
using System.Linq;
using CloudNativeKit.Domain;
using Microsoft.EntityFrameworkCore;

namespace CloudNativeKit.Infrastructure.DataPersistence.EfCore.Query
{
    public class QueryRepositoryFactory : IQueryRepositoryFactory
    {
        private readonly DbContext _context;
        private ConcurrentDictionary<Type, object> _repositories;

        public QueryRepositoryFactory(DbContext context)
        {
            _context = context;
        }

        public IQueryRepository<TEntity, TId> QueryRepository<TEntity, TId>() where TEntity : class, IAggregateRoot<TId>
        {
            if (_repositories == null)
                _repositories = new ConcurrentDictionary<Type, object>();

            if (!_repositories.ContainsKey(typeof(TEntity)))
            {
                var cachedRepo = new QueryRepository<TEntity, TId>(_context);
                _repositories[typeof(TEntity)] = cachedRepo;
            }

            return (IQueryRepository<TEntity, TId>)_repositories[typeof(TEntity)];
        }
    }

    public class QueryRepository<TEntity, TId> : QueryRepository<DbContext, TEntity, TId>
        where TEntity : class, IAggregateRoot<TId>
    {
        public QueryRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }

    public class QueryRepository<TDbContext, TEntity, TId> : IQueryRepository<TEntity, TId>
        where TDbContext : DbContext
        where TEntity : class, IAggregateRoot<TId>
    {
        private readonly TDbContext _dbContext;

        public QueryRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> Queryable()
        {
            return _dbContext.Set<TEntity>();
        }
    }
}
