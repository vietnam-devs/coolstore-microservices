using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using N8T.Domain;
using N8T.Infrastructure.Helpers;
using SaleService.Domain.Gateway;
using SaleService.Domain.Model;
using SaleService.Infrastructure.Data;

namespace SaleService.Application.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderQuery, bool>
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
        private readonly IUserGateway _userGateway;
        private readonly IOrderValidationService _orderValidationService;

        public CreateOrderHandler(IDbContextFactory<MainDbContext> dbContextFactory, IUserGateway userGateway, IOrderValidationService orderValidationService)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _userGateway = userGateway ?? throw new ArgumentNullException(nameof(userGateway));
            _orderValidationService = orderValidationService ?? throw new ArgumentNullException(nameof(orderValidationService));
        }

        public async Task<bool> Handle(CreateOrderQuery request, CancellationToken cancellationToken)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var cart = request.Cart;
            if (cart is null)
            {
                throw new CoreException("Couldn't find any cart information.");
            }

            var user = await _userGateway.GetUserInfo(cart.UserId, cancellationToken);
            if (user is null)
            {
                throw new CoreException($"Couldn't find any user with id={cart.UserId}.");
            }

            var inventoryIds =  cart.Items.Select(x => x.InventoryId).Distinct();
            var isInventoryValid = await _orderValidationService.ValidateInventoriesAsync(inventoryIds, cancellationToken);
            if (!isInventoryValid)
            {
                throw new Exception("Invalid inventories in orders!");
            }

            var productIds = cart.Items.Select(x => x.ProductId).Distinct();
            var isProductValid = await _orderValidationService.ValidateProductsAsync(productIds, cancellationToken);
            if (!isProductValid)
            {
                throw new Exception("Invalid products in orders!");
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = user.Id,
                CustomerFullName = user.FullName,
                CustomerEmail = user.Email,
                CustomerAddress = user.Address,
                OrderDate = DateTimeHelper.NewDateTime(), // get the date created order
                OrderStatus = OrderStatus.Received
            };

            var orderItems = new List<OrderItem>();
            foreach (var cartItem in cart.Items)
            {
                orderItems.Add(new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = cartItem.ProductId,
                    ProductName = cartItem.ProductName,
                    Price = (decimal)cartItem.ProductPrice,
                    Quantity = cartItem.Quantity,
                    InventoryId = cartItem.InventoryId,
                    InventoryFullInfo = $"{cartItem.InventoryLocation}: {cartItem.InventoryDescription}"
                });
            }

            order.OrderItems = orderItems;
            await dbContext.Orders.AddAsync(order, cancellationToken);

            // commit unit of work
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
