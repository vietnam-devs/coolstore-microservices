using Newtonsoft.Json.Linq;

namespace VND.CoolStore.Services.GraphQL.v1
{
    public class OperationRequest
    {
        public string OperationName { get; set; }

        public string Query { get; set; }

        public JObject Variables { get; set; }
    }
}
