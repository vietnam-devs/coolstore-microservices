using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Application.GetProductsByPriceAndName;

namespace ProductCatalogService.Api.Http.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        [Authorize]
        [HttpGet("{page}/{price}")]
        public async Task<IEnumerable<GetProductsByPriceAndNameItem>> Get([FromServices] IMediator mediator,
            int page, double price) =>
            await mediator.Send(new GetProductsByPriceAndNameQuery {Page = page, Price = price});
    }
}
