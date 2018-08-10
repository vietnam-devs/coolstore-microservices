using VND.Fw.Domain;
using VND.Fw.Infrastructure.EfCore.Repository;

namespace VND.Fw.Infrastructure.EfCore.Extensions
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
