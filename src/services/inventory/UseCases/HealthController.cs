using Microsoft.AspNetCore.Mvc;

namespace VND.Services.Inventory.UseCases
{
    [Route("")]
    [ApiVersionNeutral]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HealthController : Controller
    {
        [HttpGet("/health")]
        public bool Get()
        {
            return true;
        }
    }
}
