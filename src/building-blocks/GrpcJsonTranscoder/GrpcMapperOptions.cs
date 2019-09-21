using System.Collections.Generic;

namespace GrpcJsonTranscoder
{
    public class GrpcMapperOptions
    {
        public List<GrpcLookupOption> GrpcMappers { get; set; }

        public class GrpcLookupOption
        {
            public string GrpcMethod { get; set; }
            public string GrpcHost { get; set; }
        }
    }
}
