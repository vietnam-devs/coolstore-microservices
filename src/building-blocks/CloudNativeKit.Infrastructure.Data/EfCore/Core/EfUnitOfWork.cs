using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Domain;
using Microsoft.EntityFrameworkCore;

namespace CloudNativeKit.Infrastructure.Data.EfCore.Core
{
    public interface IEfUnitOfWork : IUnitOfWork { }
    public interface IEfUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext { }

    public class EfUnitOfWork<TDbContext> : IEfUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        private ConcurrentDictionary<string, object> _repositories;

        public EfUnitOfWork(TDbContext context)
        {
            _context = context;
        }

        public IQueryRepository<TEntity, TId> QueryRepository<TEntity, TId>() where TEntity : class, IAggregateRoot<TId>
        {
            if (_repositories == null)
                _repositories = new ConcurrentDictionary<string, object>();

            var key = $"{typeof(TEntity)}-query";
            if (!_repositories.ContainsKey(key))
            {
                var cachedRepo = new QueryRepository<TEntity, TId>(_context);
                _repositories[key] = cachedRepo;
            }

            return (IQueryRepository<TEntity, TId>)_repositories[key];
        }

        public virtual IRepositoryAsync<TEntity, TId> RepositoryAsync<TEntity, TId>() where TEntity : class, IAggregateRoot<TId>
        {
            if (_repositories == null) _repositories = new ConcurrentDictionary<string, object>();

            var key = $"{typeof(TEntity)}-command";
            if (!_repositories.ContainsKey(key))
                _repositories[key] = new RepositoryAsync<DbContext, TEntity, TId>(_context);

            return (IRepositoryAsync<TEntity, TId>)_repositories[key];
        }

        public virtual int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context?.Dispose();
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

    public class RepositoryAsync<TEntity, TId> : RepositoryAsync<DbContext, TEntity, TId>, IRepositoryAsync<TEntity, TId>
        where TEntity : class, IAggregateRoot<TId>
    {
        public RepositoryAsync(DbContext dbContext) : base(dbContext)
        {
        }
    }

    public class RepositoryAsync<TDbContext, TEntity, TId> : IRepositoryAsync<TEntity, TId>
        where TDbContext : DbContext
        where TEntity : class, IAggregateRoot<TId>
    {
        private readonly TDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryAsync(TDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            var entry = _dbSet.Remove(entity);
            return await Task.FromResult(1);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var entry = _dbContext.Entry(entity);
            entry.State = EntityState.Modified;
            return await Task.FromResult(entry.Entity);
        }
    }
}
