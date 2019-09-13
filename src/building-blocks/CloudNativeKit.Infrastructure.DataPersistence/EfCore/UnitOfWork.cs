using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Domain;
using Microsoft.EntityFrameworkCore;

namespace CloudNativeKit.Infrastructure.DataPersistence.EfCore
{
    public class UnitOfWork : IUnitOfWorkAsync
    {
        private readonly DbContext _context;
        private ConcurrentDictionary<Type, object> _repositories;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public virtual IRepositoryAsync<TEntity, TId> RepositoryAsync<TEntity, TId>() where TEntity : class, IAggregateRoot<TId>
        {
            if (_repositories == null) _repositories = new ConcurrentDictionary<Type, object>();

            if (!_repositories.ContainsKey(typeof(TEntity)))
                _repositories[typeof(TEntity)] = new RepositoryAsync<DbContext, TEntity, TId>(_context);

            return (IRepositoryAsync<TEntity, TId>)_repositories[typeof(TEntity)];
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

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            var entry = _dbSet.Remove(entity);
            return await Task.FromResult(entry.Entity);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var entry = _dbContext.Entry(entity);
            entry.State = EntityState.Modified;
            return await Task.FromResult(entry.Entity);
        }
    }
}
