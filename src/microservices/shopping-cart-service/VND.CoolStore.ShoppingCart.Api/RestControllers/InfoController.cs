using CloudNativeKit.Infrastructure.SysInfo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static CloudNativeKit.Infrastructure.SysInfo.ConfigurationExtensions;

namespace VND.CoolStore.ShoppingCart.Api.RestControllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoController : ControllerBase
    {
        private readonly IConfiguration _config;

        public InfoController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("/info")]
        public ActionResult<SysInfoModel> Info()
        {

            return new JsonResult(_config.GetSystemInformation());
        }
    }
}
