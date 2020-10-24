using System.Collections.Generic;

namespace N8T.Infrastructure.GraphQL.OffsetPaging
{
    public class OffsetPaging<TValueType>
    {
        public IEnumerable<TValueType> Edges { get; set; }
        public long? TotalCount { get; set; }
    }
}
