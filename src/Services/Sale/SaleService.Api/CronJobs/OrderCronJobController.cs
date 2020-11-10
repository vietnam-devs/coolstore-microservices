using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SaleService.Application.CompleteOrder;
using SaleService.Application.ProcessOrder;

namespace SaleService.Api.CronJobs
{
    [ApiController]
    public class OrderCronJobController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderCronJobController> _logger;

        public OrderCronJobController(IMediator mediator, ILogger<OrderCronJobController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("cron-complete-order")]
        public async Task ProcessOrder()
        {
            _logger.LogInformation($"[30s] Cron @{DateTime.Now}");

            await _mediator.Send(new ProcessOrderQuery());
        }

        [HttpPost("cron-process-order")]
        public async Task CompleteOrder()
        {
            _logger.LogInformation($"[30s] Cron @{DateTime.Now}");

            await _mediator.Send(new CompleteOrderQuery());
        }
    }
}
