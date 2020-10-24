using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using N8T.Infrastructure.Status;

namespace ProductCatalogService.Api.Http.Controllers
{
    [ApiController]
    [Route("")]
    public class InfoController : ControllerBase
    {
        [HttpGet("/info")]
        public IActionResult Status([FromServices] IConfiguration config) => Content(config.BuildAppStatus());
    }
}
