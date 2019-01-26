using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace VND.CoolStore.Services.OpenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/routes")]
        public IEnumerable<RouteCollection> Get()
        {
            var routes = RouteData.Routers.OfType<RouteCollection>().AsEnumerable();
            return routes;
        }
    }
}
