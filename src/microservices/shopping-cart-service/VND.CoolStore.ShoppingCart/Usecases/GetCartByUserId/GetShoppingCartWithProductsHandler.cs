using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.Dapper.Core;
using CloudNativeKit.Utils.Extensions;
using Dapper;
using MediatR;
using VND.CoolStore.ShoppingCart.DataContracts.Api.V1;
using VND.CoolStore.ShoppingCart.DataContracts.Dto.V1;
using VND.CoolStore.ShoppingCart.Domain.ProductCatalog;

namespace VND.CoolStore.ShoppingCart.Usecases.GetCartByUserId
{
    public class GetCartByUserIdHandler : IRequestHandler<GetCartByUserIdRequest, GetCartByUserIdResponse>
    {
        private readonly IDapperUnitOfWork _unitOfWork;
        private readonly IInventoryGateway _inventoryGateway;

        public GetCartByUserIdHandler(IDapperUnitOfWork unitOfWork, IInventoryGateway inventoryGateway)
        {
            _unitOfWork = unitOfWork;
            _inventoryGateway = inventoryGateway;
        }

        public async Task<GetCartByUserIdResponse> Handle(GetCartByUserIdRequest request, CancellationToken cancellationToken)
        {
            using var conn = _unitOfWork.SqlConnectionFactory.GetOpenConnection();

            // query from database
            // ref at https://stackoverflow.com/questions/966176/select-distinct-on-one-column
            var views = await conn.QueryAsync<CartWithProductsRow>(
                @"
                SELECT *
                FROM (
                    SELECT c.Id CartIdGuid, c.UserId UserIdGuid, c.CartItemTotal, c.CartTotal,
                        c.CartItemPromoSavings, c.ShippingTotal, c.ShippingPromoSavings,
                        c.IsCheckout, ci.Quantity, pc.ProductId ProductIdGuid, pc.Name ProductName,
                        pc.Price ProductPrice, pc.[Desc] ProductDesc, pc.ImagePath ProductImagePath,
                        pc.InventoryId InventoryIdGuid, ROW_NUMBER() OVER (PARTITION BY pc.ProductId ORDER BY c.Id) AS RowNumber
                    FROM [cart].Carts c
	                    INNER JOIN [cart].CartItems ci ON c.Id = ci.CurrentCartId
	                    INNER JOIN [cart].ProductCatalogIds pci ON ci.Id = pci.CurrentCartItemId
	                    INNER JOIN [catalog].ProductCatalogs pc ON pci.ProductId = pc.ProductId
                    WHERE c.UserId = @UserId
                ) as temp
                WHERE temp.RowNumber = 1",
                new { UserId = request.UserId.ConvertTo<Guid>() });

            if (views.Count() <= 0)
            {
                return new GetCartByUserIdResponse();
            }

            var inventories = await _inventoryGateway.GetAvailabilityInventories();

            // process for transformation data with additional information
            views = views.Select(row =>
            {
                row.CartId = row.CartIdGuid.ToString();
                row.UserId = row.UserIdGuid.ToString();
                row.ProductId = row.ProductIdGuid.ToString();
                row.InventoryId = row.InventoryIdGuid.ToString();
                var inv = inventories.FirstOrDefault(x => x.Id == row.InventoryId);
                if (inv != null)
                {
                    row.InventoryLocation = inv.Location;
                    row.InventoryDescription = inv.Description;
                    row.InventoryWebsite = inv.Website;
                }
                return row;
            });

            var firstRow = views.FirstOrDefault();

            var result = new GetCartByUserIdResponse {
                Cart = new CartDto {
                    Id = firstRow.CartId,
                    UserId = firstRow.UserId,
                    CartItemTotal = firstRow.CartItemTotal,
                    CartItemPromoSavings = firstRow.CartItemPromoSavings,
                    ShippingTotal = firstRow.ShippingTotal,
                    ShippingPromoSavings = firstRow.ShippingPromoSavings,
                    CartTotal = firstRow.CartTotal,
                    IsCheckOut = firstRow.IsCheckOut
                }
            };

            var cartItems = views.Select(x => new CartItemDto {
                Quantity = x.Quantity,
                Price = x.ProductPrice,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProductPrice = x.ProductPrice,
                ProductDesc = x.ProductDesc,
                ProductImagePath = x.ProductImagePath,
                InventoryId = x.InventoryId,
                InventoryLocation = x.InventoryLocation,
                InventoryDescription = x.InventoryDescription,
                InventoryWebsite = x.InventoryWebsite
            });

            result.Cart.Items.AddRange(cartItems);
            return result;
        }
    }
}
