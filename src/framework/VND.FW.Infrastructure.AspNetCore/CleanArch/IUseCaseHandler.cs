using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VND.Fw.Domain;

namespace VND.FW.Infrastructure.AspNetCore.CleanArch
{
  public interface IEventHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
  {
    IQueryRepositoryFactory QueryRepositoryFactory { get; }
    IUnitOfWorkAsync UnitOfWork { get; }
  }

  public abstract class EventHandlerBase<TRequest, TResponse> : IEventHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
  {
    protected EventHandlerBase(IUnitOfWorkAsync uow, IQueryRepositoryFactory queryRepositoryFactory)
    {
      QueryRepositoryFactory = queryRepositoryFactory;
      UnitOfWork = uow;
    }

    public IQueryRepositoryFactory QueryRepositoryFactory { get; }

    public IUnitOfWorkAsync UnitOfWork { get; }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
  }
}
