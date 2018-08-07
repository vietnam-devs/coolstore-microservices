using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using MediatR;

namespace VND.FW.Infrastructure.AspNetCore.CleanArch
{
  public static class EventAggregatorExtensions
  {
    public static IObservable<dynamic> SendStream<TRequest, TResponse>(
      this IMediator mediator,
      TRequest request,
      Func<TResponse, dynamic> mapTo = null)
      where TRequest : IRequest<TResponse>
      where TResponse : class
    {
      return mediator.Send(request)
        .ToObservable()
        .Select(x => x.PresentFor(mapTo));
    }
  }
}
