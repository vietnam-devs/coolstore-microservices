using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N8T.Infrastructure.App.Dtos;
using ProductCatalogService.Application.GetDetailOfSpecificProduct;
using ProductCatalogService.Application.GetProductsByPriceAndName;

namespace ProductCatalogService.Api.Http.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        [Authorize]
        [HttpGet("{page}/{price}")]
        public async Task<IEnumerable<FlatProductDto>> Get([FromServices] IMediator mediator,
            int page, double price) =>
            await mediator.Send(new GetProductsByPriceAndNameQuery {Page = page, Price = price});

        [Authorize]
        [HttpGet("{id}")]
        public async Task<FlatProductDto> Get([FromServices] IMediator mediator, Guid id) =>
            await mediator.Send(new GetDetailOfSpecificProductQuery {Id = id});
    }
}
