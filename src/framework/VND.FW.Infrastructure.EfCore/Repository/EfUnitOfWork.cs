using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using VND.Fw.Domain;

namespace VND.FW.Infrastructure.EfCore.Repository
{
  public class EfUnitOfWork : IUnitOfWorkAsync
  {
    private readonly DbContext _context;
    protected IDbContextTransaction Transaction;
    protected IServiceProvider ServiceProvider;

    public EfUnitOfWork(DbContext context, IServiceProvider serviceProvider)
    {
      _context = context;
      ServiceProvider = serviceProvider;
    }

    public virtual IRepositoryAsync<TEntity> Repository<TEntity>() where TEntity : IEntity
    {
      return (IEfRepositoryAsync<TEntity>)ServiceProvider.GetService(typeof(IEfRepositoryAsync<TEntity>));
    }

    public int? CommandTimeout
    {
      get => _context.Database.GetCommandTimeout();
      set => _context.Database.SetCommandTimeout(value);
    }

    public virtual int SaveChanges() => _context.SaveChanges();

    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
      return _context.SaveChangesAsync(cancellationToken);
    }

    public virtual int ExecuteSqlCommand(string sql, params object[] parameters)
    {
      return _context.Database.ExecuteSqlCommand(sql, parameters);
    }

    public virtual async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
    {
      return await _context.Database.ExecuteSqlCommandAsync(sql, parameters);
    }

    public virtual async Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken, params object[] parameters)
    {
      return await _context.Database.ExecuteSqlCommandAsync(sql, cancellationToken, parameters);
    }

    public void Dispose()
    {
      Transaction?.Dispose();
      _context?.Dispose();
    }
  }
}
