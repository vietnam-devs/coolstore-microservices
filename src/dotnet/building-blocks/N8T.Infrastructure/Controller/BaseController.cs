using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace N8T.Infrastructure.Controller
{
    public class BaseController : Microsoft.AspNetCore.Mvc.Controller
    {
        private ISender _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
    }
}
