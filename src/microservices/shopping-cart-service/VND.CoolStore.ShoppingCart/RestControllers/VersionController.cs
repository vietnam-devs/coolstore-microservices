using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace VND.CoolStore.ShoppingCart.RestControllers
{
    [ApiController]
    [Route("[controller]")]
    public class VersionController : ControllerBase
    {
        [HttpGet("/version")]
        public ActionResult<string> Version()
        {
            var informationVersion = Assembly.GetEntryAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;

            return informationVersion;
        }
    }
}
