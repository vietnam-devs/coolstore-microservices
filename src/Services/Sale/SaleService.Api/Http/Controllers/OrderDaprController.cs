using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using N8T.Infrastructure.App.Requests.Order;
using SaleService.Application.UpdateOrderStatus;
using SaleService.Domain.Model;

namespace SaleService.Api.Http.Controllers
{
    [ApiController]
    public class OrderDaprController : ControllerBase
    {
        [HttpPost("/update-order-status")]
        public async Task<bool> GetProducts(OrderWithStatusRequest request,
            [FromServices] IMediator mediator) =>
            await mediator.Send(new UpdateOrderStatusQuery
            {
                OrderId = request.OrderId, OrderStatus = (OrderStatus)request.OrderStatus
            });
    }
}
