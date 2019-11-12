using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GrpcJsonTranscoder.Internal.Http
{
    public class GrpcHttpContent : HttpContent
    {
        private readonly string _result;

        public GrpcHttpContent(string result)
        {
            _result = result;
        }

        public GrpcHttpContent(object result)
        {
            _result = JsonConvert.SerializeObject(result);
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            var writer = new StreamWriter(stream);

            await writer.WriteAsync(_result);

            await writer.FlushAsync();
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _result.Length;

            return true;
        }
    }
}
