using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcJsonTranscoder.Internal.Http
{
    internal static class HttpContextExtensions
    {
        public static string ParseGetJsonRequest(this HttpContext context, IDictionary<string, string> upstreamHeaders = null)
        {
            var o = new JObject();

            if (upstreamHeaders != null && upstreamHeaders.ContainsKey("x-grpc-route-data"))
            {
                // route data
                var nameValues = JsonConvert.DeserializeObject<List<NameAndValue>>(upstreamHeaders["x-grpc-route-data"]); // work with ocelot
                foreach (var nameValue in nameValues)
                {
                    o.Add(nameValue.Name.Replace("{", "").Replace("}", ""), nameValue.Value);
                }
            }

            // query string
            foreach (var q in context.Request.Query)
            {
                o.Add(q.Key, q.Value.ToString());
            }

            return JsonConvert.SerializeObject(o);
        }

        public static string ParseOtherJsonRequest(this HttpContext context, IDictionary<string, string> upstreamHeaders)
        {
            var json = string.Empty;
            if (upstreamHeaders != null && upstreamHeaders.ContainsKey("x-grpc-body-data"))
            {
                // route data
                json = upstreamHeaders["x-grpc-body-data"]; // work with ocelot
            }

            return json == string.Empty ? "{}" : json;
        }

        public static string ParseGetJsonRequestOnAggregateService(this HttpContext context)
        {
            var o = new JObject();

            if (context.Request.Headers.ContainsKey("x-grpc-route-data"))
            {
                // route data
                var nameValues = JsonConvert.DeserializeObject<List<NameAndValue>>(context.Request.Headers["x-grpc-route-data"]); // work with ocelot
                foreach (var nameValue in nameValues)
                {
                    o.Add(nameValue.Name.Replace("{", "").Replace("}", ""), nameValue.Value);
                }
            }

            // query string
            foreach (var q in context.Request.Query)
            {
                o.Add(q.Key, q.Value.ToString());
            }

            return JsonConvert.SerializeObject(o);
        }

        public static async Task<string> ParseOtherJsonRequestOnAggregateService(this HttpContext context)
        {
            // ref at https://stackoverflow.com/questions/43403941/how-to-read-asp-net-core-response-body
            var encoding = context.Request.GetTypedHeaders().ContentType?.Encoding ?? Encoding.UTF8;
            var stream = new StreamReader(context.Request.Body, encoding);
            var json = await stream.ReadToEndAsync();
            return json == string.Empty ? "{}" : json;
        }

        public static IDictionary<string, string> GetRequestHeaders(this HttpContext context)
        {
            var headers = new Dictionary<string, string>();
            foreach (string key in context.Request.Headers.Keys)
            {
                if (key.StartsWith(":"))
                {
                    continue;
                }
                if (key.StartsWith("grpc-", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                else if (key.ToLowerInvariant() == "content-type" || key.ToLowerInvariant() == "authorization")
                {
                    //todo: investigate it more
                    var value = context.Request.Headers[key];
                    headers.Add(key, value.FirstOrDefault());
                }
            }

            return headers;
        }
    }
}
