using VND.Fw.Domain;

namespace VND.FW.Infrastructure.EfCore.Service
{
  public interface IService
  {
  }

  public interface IQueryService : IService
  {
    IQueryRepositoryFactory QueryRepositoryFactory { get; }
  }

  public interface ICommandService : IService
  {
    IUnitOfWorkAsync UnitOfWork { get; }
  }
}
