using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VND.Fw.Domain;

namespace VND.Fw.Infrastructure.AspNetCore.CleanArch
{
  public interface IEventHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
  {
    IQueryRepositoryFactory QueryRepositoryFactory { get; }
    IUnitOfWorkAsync UnitOfWork { get; }
  }

  public abstract class TxRequestHandlerBase<TRequest, TResponse> : IEventHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
  {
    protected TxRequestHandlerBase(IUnitOfWorkAsync uow, IQueryRepositoryFactory queryRepositoryFactory)
    {
      QueryRepositoryFactory = queryRepositoryFactory;
      UnitOfWork = uow;
    }

    public IQueryRepositoryFactory QueryRepositoryFactory { get; }

    public IUnitOfWorkAsync UnitOfWork { get; }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
      var result = await TxHandle(request, cancellationToken);
      await UnitOfWork.SaveChangesAsync(cancellationToken);
      return result;
    }

    public abstract Task<TResponse> TxHandle(TRequest request, CancellationToken cancellationToken);
  }

  public abstract class RequestHandlerBase<TRequest, TResponse> : IEventHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
  {
    protected RequestHandlerBase(IUnitOfWorkAsync uow, IQueryRepositoryFactory queryRepositoryFactory)
    {
      QueryRepositoryFactory = queryRepositoryFactory;
      UnitOfWork = uow;
    }

    public IQueryRepositoryFactory QueryRepositoryFactory { get; }

    public IUnitOfWorkAsync UnitOfWork { get; }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
  }
}
