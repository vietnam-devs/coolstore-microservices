using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using N8T.Infrastructure.App.Dtos;
using N8T.Infrastructure.App.Requests.ProductCatalog;
using ProductCatalogService.Application.GetProductsByIds;

namespace ProductCatalogService.Api.Http.Controllers
{
    [ApiController]
    public class ProductDaprController : ControllerBase
    {
        [HttpPost("/get-products-by-ids")]
        public async Task<IEnumerable<ProductDto>> GetProducts(ProductByIdsRequest request,
            [FromServices] IMediator mediator) =>
            await mediator.Send(new GetProductsByIdsQuery {ProductIds = request.ProductIds});

        [HttpPost("/get-product-by-id")]
        public async Task<ProductDto> GetProduct(ProductByIdRequest request, [FromServices] IMediator mediator)
        {
            var result =
                await mediator.Send(new GetProductsByIdsQuery {ProductIds = new List<Guid>(new[] {request.Id})});

            return result?.FirstOrDefault()!;
        }
    }
}
