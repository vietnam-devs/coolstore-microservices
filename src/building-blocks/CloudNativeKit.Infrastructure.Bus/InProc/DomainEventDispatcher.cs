using System;
using System.Threading.Tasks;
using CloudNativeKit.Domain;
using CloudNativeKit.Infrastructure.Bus.Messaging;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CloudNativeKit.Infrastructure.Bus.InProc
{
    public class DomainEventDispatcher<TDbContext> : IDomainEventDispatcher where TDbContext : DbContext
    {
        private readonly IMediator _mediator;
        private readonly IEfUnitOfWork<TDbContext> _unitOfWork;

        public DomainEventDispatcher(IMediator mediator, IEfUnitOfWork<TDbContext> uow)
        {
            _mediator = mediator;
            _unitOfWork = uow;
        }

        public async Task Dispatch(IEvent @event)
        {
            var repo = _unitOfWork.RepositoryAsync<Outbox, Guid>();

            var oubox = new Outbox(
                @event.Id,
                @event.OccurredOn,
                @event);

            await repo.AddAsync(oubox);
            await _unitOfWork.SaveChangesAsync(default);

            await _mediator.Publish(new NotificationEnvelope(@event));
        }

        public void Dispose()
        {
        }
    }
}
