using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Domain;
using Microsoft.EntityFrameworkCore;
using ReflectionMagic;

namespace CloudNativeKit.Infrastructure.Data.EfCore.Core
{
    public abstract class AppDbContext : DbContext
    {
        private readonly IEnumerable<IDomainEventDispatcher> _eventBuses = null;

        protected AppDbContext(DbContextOptions options, IEnumerable<IDomainEventDispatcher> eventBuses = null)
            : base(options)
        {
            _eventBuses = eventBuses;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = base.SaveChangesAsync(cancellationToken);
            SaveChangesWithEvents();
            return result;
        }

        public override int SaveChanges()
        {
            var result = base.SaveChanges();
            SaveChangesWithEvents();
            return result;
        }

        /// <summary>
        /// Source: https://github.com/ardalis/CleanArchitecture/blob/master/src/CleanArchitecture.Infrastructure/Data/AppDbContext.cs
        /// </summary>
        private void SaveChangesWithEvents()
        {
            var entities = ChangeTracker.Entries().Select(e => e.Entity);

            var entitiesWithEvents = ChangeTracker
                .Entries()
                .Select(e => e.Entity)
                .Where(e => e.GetType().BaseType.IsGenericType
                    && e.GetType().BaseType.GetGenericTypeDefinition().IsAssignableFrom(typeof(AggregateRootBase<>)))
                .Where(e => e.AsDynamic().GetUncommittedEvents().Count > 0)
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var rootAggregator = entity.AsDynamic();
                var events = rootAggregator.GetUncommittedEvents();
                foreach (var @event in events)
                    _eventBuses.Select(b => b.Dispatch(@event)).ToList();
                rootAggregator.ClearUncommittedEvents();
            }
        }
    }
}
