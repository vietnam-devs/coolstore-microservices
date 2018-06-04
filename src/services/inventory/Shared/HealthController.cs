using Microsoft.AspNetCore.Mvc;

namespace VND.Services.Inventory.v1
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