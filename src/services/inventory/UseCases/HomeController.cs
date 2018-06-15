using Microsoft.AspNetCore.Mvc;

namespace VND.Services.Inventory.UseCases
{
    [Route("")]
    [ApiVersionNeutral]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}