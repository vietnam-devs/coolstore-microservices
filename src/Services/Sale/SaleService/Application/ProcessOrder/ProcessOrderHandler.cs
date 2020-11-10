using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using N8T.Infrastructure.Helpers;
using SaleService.Domain.Model;
using SaleService.Infrastructure.Data;

namespace SaleService.Application.ProcessOrder
{
    public class ProcessOrderHandler : IRequestHandler<ProcessOrderQuery, bool>
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
        private readonly ILogger<ProcessOrderHandler> _logger;

        public ProcessOrderHandler(IDbContextFactory<MainDbContext> dbContextFactory, ILogger<ProcessOrderHandler> logger)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(ProcessOrderQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Prefix} Handle ProcessOrderQuery", nameof(ProcessOrderHandler));

            await using var dbContext = _dbContextFactory.CreateDbContext();

            var orders = dbContext.Orders
                .Include(x => x.OrderItems)
                .Where(x => x.OrderStatus == OrderStatus.Received);

            foreach (var order in orders)
            {
                var seconds = DateTimeHelper.NewDateTime().Subtract(order.Created).Seconds;
                _logger.LogInformation("{Prefix} {Seconds}s until now...", nameof(ProcessOrderHandler), seconds);

                if (seconds >= 30) // after 30 seconds
                {
                    order.OrderStatus = OrderStatus.Processing;
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("{Prefix} Update all orders with status Received->Processing",
                nameof(ProcessOrderHandler));

            return true;
        }
    }
}
