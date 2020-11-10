using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N8T.Infrastructure.App.Dtos;
using SaleService.Application.GetOrderListByUser;

namespace SaleService.Api.Http.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class ProductController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<OrderDto>> Get([FromServices] IMediator mediator) =>
            await mediator.Send(new GetOrderListByUserQuery());
    }
}
