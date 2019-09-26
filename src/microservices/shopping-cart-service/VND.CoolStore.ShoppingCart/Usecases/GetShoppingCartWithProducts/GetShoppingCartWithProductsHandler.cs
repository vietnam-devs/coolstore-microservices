using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.Dapper.Core;
using CloudNativeKit.Utils.Extensions;
using Dapper;
using MediatR;
using VND.CoolStore.ShoppingCart.DataContracts.V1;

namespace VND.CoolStore.ShoppingCart.Usecases.GetShoppingCartWithProducts
{
    public class GetShoppingCartWithProductsHandler : IRequestHandler<GetCartRequest, GetCartResponse>
    {
        private readonly IDapperUnitOfWork _unitOfWork;

        public GetShoppingCartWithProductsHandler(IDapperUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetCartResponse> Handle(GetCartRequest request, CancellationToken cancellationToken)
        {
            using var conn = _unitOfWork.SqlConnectionFactory.GetOpenConnection();
            var views = await conn.QueryAsync<CartWithProductsRow>(
                @"SELECT c.Id CartIdGuid, c.CartItemTotal, c.CartTotal, c.CartItemPromoSavings, c.ShippingTotal,
                    c.ShippingPromoSavings, c.IsCheckout, ci.Quantity, pc.ProductId ProductIdGuid, pc.Name ProductName,
                    pc.Price ProductPrice, pc.[Desc] ProductDesc, pc.ImagePath ProductImagePath
                FROM [cart].Carts c 
	                INNER JOIN [cart].CartItems ci ON c.Id = ci.CurrentCartId
	                INNER JOIN [cart].ProductCatalogIds pci ON ci.Id = pci.CurrentCartItemId
	                INNER JOIN [catalog].ProductCatalogs pc ON pci.ProductId = pc.ProductId
                WHERE c.Id = @CartId", new { CartId = request.CartId.ConvertTo<Guid>() });

            views = views.Select(row => {
                row.CartId = row.CartIdGuid.ToString();
                row.ProductId = row.ProductIdGuid.ToString();
                return row;
            });

            var result = new GetCartResponse();
            result.Rows.AddRange(views);

            return result;
        }
    }
}
