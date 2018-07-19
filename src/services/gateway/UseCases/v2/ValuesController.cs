using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace VND.CoolStore.Services.ApiGateway.UseCases.v2
{
	[ApiVersion("2.0")]
	[Route("api/v{api-version:apiVersion}/[controller]")]
	public class ValuesController : FW.Infrastructure.AspNetCore.ControllerBase
		{
				// GET api/values
				[HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
