using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tanka.graphql;
using tanka.graphql.server.utilities;
using static tanka.graphql.Executor;
using static tanka.graphql.Parser;

namespace VND.CoolStore.Services.GraphQL.v1
{
    [Route("api/graphql")]
    public class QueryController : Controller
    {
        private readonly CoolStoreSchema _schema;

        public QueryController(CoolStoreSchema schema)
        {
            _schema = schema;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OperationRequest request)
        {
            var result = await ExecuteAsync(new ExecutionOptions
            {
                Document = ParseDocument(request.Query),
                Schema = _schema.CoolStore,
                OperationName = request.OperationName,
                VariableValues = request.Variables?.ToVariableDictionary()
            });

            return Ok(result);
        }
    }
}
