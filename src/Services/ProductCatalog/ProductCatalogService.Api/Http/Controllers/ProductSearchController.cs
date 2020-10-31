using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Application.SearchProducts;

namespace ProductCatalogService.Api.Http.Controllers
{
    [ApiController]
    [Route("api/product-search")]
    public class ProductSearchController : ControllerBase
    {
        [Authorize]
        [HttpGet("{query}/{price}/{page}/{pageSize}")]
        public async Task<SearchProductsResponse> Get([FromServices] IMediator mediator,
            string query, double price, int page = 1, int pageSize = 20) =>
            await mediator.Send(
                new SearchProductsQuery {Query = query, Price = price, Page = page, PageSize = pageSize});
    }
}
