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
        private readonly IOrderValidationService _orderValidationService;
        private readonly ILogger<CompleteOrderHandler> _logger;

        public CompleteOrderHandler(IDbContextFactory<MainDbContext> dbContextFactory, IOrderValidationService orderValidationService, ILogger<CompleteOrderHandler> logger)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _orderValidationService = orderValidationService ?? throw new ArgumentNullException(nameof(orderValidationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(CompleteOrderQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Prefix} Handle CompleteOrderQuery", nameof(CompleteOrderHandler));

            await using var dbContext = _dbContextFactory.CreateDbContext();

            var orders = dbContext.Orders
                .Include(x => x.OrderItems)
                .Where(x => x.OrderStatus == OrderStatus.Processing);

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
                var minutes = DateTimeHelper.NewDateTime().Subtract(order.Created).Minutes;
                _logger.LogInformation("{Prefix} {Minutes}s until now...", nameof(CompleteOrderHandler), minutes);

                if (minutes >= 1) // after 1 minute
                {
                    order.OrderStatus = OrderStatus.Complete;
                    order.CompleteDate = DateTimeHelper.NewDateTime();
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("{Prefix} Update all orders with status Received->Complete",
                nameof(CompleteOrderHandler));

            return true;
        }
    }
}
