using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace VND.FW.Infrastructure.AspNetCore
{
    public static class HttpRequestExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> GetOpenTracingInfo(this HttpRequest request)
        {
            return request.Headers.Where(x =>
                x.Key == "x-request-id" ||
                x.Key == "x-b3-traceid" ||
                x.Key == "x-b3-spanid" ||
                x.Key == "x-b3-parentspanid" ||
                x.Key == "x-b3-sampled" ||
                x.Key == "x-b3-flags" ||
                x.Key == "x-ot-span-context"
            ).Select(y =>
                new KeyValuePair<string, string>(
                    y.Key,
                    y.Value.FirstOrDefault()));
        }
    }
}