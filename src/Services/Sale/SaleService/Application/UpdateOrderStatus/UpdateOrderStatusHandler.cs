using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using N8T.Domain;
using SaleService.Domain.Model;
using SaleService.Infrastructure.Data;

namespace SaleService.Application.UpdateOrderStatus
{
    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusQuery, bool>
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
        private readonly ILogger<UpdateOrderStatusHandler> _logger;

        public UpdateOrderStatusHandler(IDbContextFactory<MainDbContext> dbContextFactory,
            ILogger<UpdateOrderStatusHandler> logger)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(UpdateOrderStatusQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Prefix} Handle UpdateOrderStatusQuery", nameof(UpdateOrderStatusHandler));

            await using var dbContext = _dbContextFactory.CreateDbContext();

            var order = dbContext.Orders
                .Include(x => x.OrderItems)
                .FirstOrDefault(x => x.Id == request.OrderId);

            if (order is null)
            {
                throw new CoreException($"Order id={request.OrderId} wasn't exists.");
            }

            if (!(request.OrderStatus == OrderStatus.Received
                || request.OrderStatus == OrderStatus.Processing
                || request.OrderStatus == OrderStatus.Complete))
            {
                throw new CoreException("Provide the incorrect order status.");
            }

            order.OrderStatus = request.OrderStatus;

            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
