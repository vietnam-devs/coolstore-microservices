using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using N8T.Infrastructure.App.Dtos;
using N8T.Infrastructure.Auth;
using SaleService.Infrastructure.Data;

namespace SaleService.Application.GetOrderListByUser
{
    public class GetOrderListByUserHandler : IRequestHandler<GetOrderListByUserQuery, IEnumerable<OrderDto>>
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
        private readonly ISecurityContextAccessor _securityContextAccessor;
        private readonly ILogger<GetOrderListByUserHandler> _logger;

        public GetOrderListByUserHandler(IDbContextFactory<MainDbContext> dbContextFactory,
            ISecurityContextAccessor securityContextAccessor,
            ILogger<GetOrderListByUserHandler> logger)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _securityContextAccessor = securityContextAccessor ?? throw new ArgumentNullException(nameof(securityContextAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOrderListByUserQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Prefix} Handle GetOrderListByUserQuery", nameof(GetOrderListByUserHandler));

            var currentUserId = _securityContextAccessor.UserId;

            await using var dbContext = _dbContextFactory.CreateDbContext();

            var orders = await dbContext.Orders
                .Include(x => x.OrderItems)
                .Where(x => x.CustomerId == currentUserId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var result = new List<OrderDto>();

            foreach (var order in orders)
            {
                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    CustomerFullName = order.CustomerFullName,
                    CustomerEmail = order.CustomerEmail,
                    CustomerAddress = order.CustomerAddress,
                    OrderStatus = (int)order.OrderStatus,
                    StaffFullName = order.StaffFullName,
                    OrderDate = order.OrderDate,
                    CompleteDate = order.CompleteDate
                };

                foreach (var orderItem in order.OrderItems)
                {
                    orderDto.OrderItems.Add(new OrderItemDto
                    {
                        Id = orderItem.Id,
                        Price = orderItem.Price,
                        Quantity = orderItem.Quantity,
                        Discount = orderItem.Discount,
                        ProductName = orderItem.ProductName,
                        InventoryFullInfo = orderItem.InventoryFullInfo
                    });
                }

                result.Add(orderDto);
            }

            return result;
        }
    }
}
