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

namespace SaleService.Application.CompleteOrder
{
    public class CompleteOrderHandler : IRequestHandler<CompleteOrderQuery, bool>
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
        private readonly ILogger<CompleteOrderHandler> _logger;

        public CompleteOrderHandler(IDbContextFactory<MainDbContext> dbContextFactory, ILogger<CompleteOrderHandler> logger)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(CompleteOrderQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Prefix} Handle CompleteOrderQuery", nameof(CompleteOrderHandler));

            await using var dbContext = _dbContextFactory.CreateDbContext();

            var orders = dbContext.Orders
                .Include(x => x.OrderItems)
                .Where(x => x.OrderStatus == OrderStatus.Processing);

            foreach (var order in orders)
            {
                order.OrderStatus = OrderStatus.Complete;
                order.CompleteDate = DateTimeHelper.NewDateTime();
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("{Prefix} Update all orders with status Received->Complete",
                nameof(CompleteOrderHandler));

            return true;
        }
    }
}
