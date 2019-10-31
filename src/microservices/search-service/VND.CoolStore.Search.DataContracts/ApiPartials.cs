using System.ComponentModel;
using MediatR;

namespace VND.CoolStore.Search.DataContracts.Api.V1
{
    [DefaultValue("DefaultReflection")]
    public static partial class SearchApiReflection
    {
    }

    public partial class SearchProductRequest : IRequest<SearchProductResponse>
    {
    }
}
