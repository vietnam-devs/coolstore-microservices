using System;
using System.Threading;
using System.Threading.Tasks;

namespace VND.Fw.Domain
{
  public interface IUnitOfWorkAsync : IRepositoryFactory, IDisposable
  {
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    int ExecuteSqlCommand(string sql, params object[] parameters);
    Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);
    Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken, params object[] parameters);
    int? CommandTimeout { get; set; }
  }
}
