using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using HotChocolate.Types.Sorting;

namespace N8T.Infrastructure.GraphQL
{
    public abstract class GraphQueryBase<TDto>
    {
        [JsonIgnore] public Expression<Func<TDto, bool>> FilterExpr { get; set; }
        [JsonIgnore] public QueryableSortVisitor SortingVisitor { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
