using VND.Fw.Domain;
using VND.FW.Infrastructure.EfCore.Repository;

namespace VND.FW.Infrastructure.EfCore.Extensions
{
  public static class EfQueryRepositoryFactoryExtensions
  {
    public static IEfQueryRepository<TEntity> QueryEfRepository<TEntity>(this IQueryRepositoryFactory factory)
      where TEntity : IEntity
    {
      return factory.QueryRepository<TEntity>() as IEfQueryRepository<TEntity>;
    }
  }
}
