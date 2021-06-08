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
        private readonly IOrderValidationService _orderValidationService;
        private readonly ILogger<ProcessOrderHandler> _logger;

        public ProcessOrderHandler(IDbContextFactory<MainDbContext> dbContextFactory, IOrderValidationService orderValidationService, ILogger<ProcessOrderHandler> logger)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _orderValidationService = orderValidationService ?? throw new ArgumentNullException(nameof(orderValidationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(ProcessOrderQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Prefix} Handle ProcessOrderQuery", nameof(ProcessOrderHandler));

            await using var dbContext = _dbContextFactory.CreateDbContext();

            var orders = dbContext.Orders
                .Include(x => x.OrderItems)
                .Where(x => x.OrderStatus == OrderStatus.Received);

            if (!orders.Any()) return false;

            // TODO: temporary to check the inventory and product to make sure it correct
            var inventoryIds = orders.SelectMany(x => x.OrderItems.Select(y => y.InventoryId)).Distinct();
            var isInventoryValid = await _orderValidationService.ValidateInventoriesAsync(inventoryIds, cancellationToken);
            if (!isInventoryValid)
            {
                throw new Exception("Invalid inventories in orders!");
            }

            var productIds = orders.SelectMany(x => x.OrderItems.Select(y => y.ProductId)).Distinct();
            var isProductValid = await _orderValidationService.ValidateProductsAsync(productIds, cancellationToken);
            if (!isProductValid)
            {
                throw new Exception("Invalid products in orders!");
            }

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
