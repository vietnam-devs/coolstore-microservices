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

namespace VND.CoolStore.ShoppingCart.Usecases.GetShoppingCartWithProducts
{
    public class GetShoppingCartWithProductsHandler : IRequestHandler<GetCartRequest, GetCartResponse>
    {
        private readonly IDapperUnitOfWork _unitOfWork;
        private readonly IInventoryGateway _inventoryGateway;

        public GetShoppingCartWithProductsHandler(IDapperUnitOfWork unitOfWork, IInventoryGateway inventoryGateway)
        {
            _unitOfWork = unitOfWork;
            _inventoryGateway = inventoryGateway;
        }

        public async Task<GetCartResponse> Handle(GetCartRequest request, CancellationToken cancellationToken)
        {
            using var conn = _unitOfWork.SqlConnectionFactory.GetOpenConnection();

            // query from database
            var views = await conn.QueryAsync<CartWithProductsRow>(
                @"SELECT c.Id CartIdGuid, c.UserId UserIdGuid, c.CartItemTotal, c.CartTotal, c.CartItemPromoSavings, c.ShippingTotal,
                    c.ShippingPromoSavings, c.IsCheckout, ci.Quantity, pc.ProductId ProductIdGuid, pc.Name ProductName,
                    pc.Price ProductPrice, pc.[Desc] ProductDesc, pc.ImagePath ProductImagePath, pc.InventoryId InventoryIdGuid
                FROM [cart].Carts c 
	                INNER JOIN [cart].CartItems ci ON c.Id = ci.CurrentCartId
	                INNER JOIN [cart].ProductCatalogIds pci ON ci.Id = pci.CurrentCartItemId
	                INNER JOIN [catalog].ProductCatalogs pc ON pci.ProductId = pc.ProductId
                WHERE c.Id = @CartId", new { CartId = request.CartId.ConvertTo<Guid>() });

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

            var result = new GetCartResponse();
            result.Rows.AddRange(views);

            return result;
        }
    }
}
