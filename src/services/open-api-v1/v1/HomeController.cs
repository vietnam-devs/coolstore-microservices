using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace VND.CoolStore.Services.OpenApiV1.v1
{
    [Route("")]
    [ApiVersionNeutral]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        private readonly string _basePath;

        public HomeController(IOptions<AppOptions> options)
        {
            var basePath = options.Value.Hosts.BasePath;
            _basePath = basePath.EndsWith('/') ? basePath.TrimEnd('/') : basePath;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Redirect($"~{_basePath}/swagger");
        }
    }
}
