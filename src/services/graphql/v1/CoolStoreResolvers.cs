using System.Collections.Generic;
using System.Threading.Tasks;
using tanka.graphql;
using tanka.graphql.resolvers;
using static tanka.graphql.resolvers.Resolve;
using VND.CoolStore.Services.GraphQL.v1.Types;

namespace VND.CoolStore.Services.GraphQL.v1
{
    public class CoolStoreResolvers : ResolverMap
    {
        public CoolStoreResolvers(ICoolStoreResolverService resolverService)
        {
            this["Sample"] = new FieldResolverMap
            {
                {"name", PropertyOf<Sample>(m => m.Name)}
            };

            this["Query"] = new FieldResolverMap
            {
                {"samples", resolverService.GetSamplesAsync}
            };
        }
    }

    public interface ICoolStoreResolverService
    {
        Task<IResolveResult> GetSamplesAsync(ResolverContext context);
    }

    public class CoolStoreResolverService : ICoolStoreResolverService
    {
        public async Task<IResolveResult> GetSamplesAsync(ResolverContext context)
        {
            var page = context.Arguments["page"] ?? -1;
            return await new ValueTask<IResolveResult>(As(new List<Sample> {new Sample {Name = $"sample at page={page}"}}));
        }
    }
}
