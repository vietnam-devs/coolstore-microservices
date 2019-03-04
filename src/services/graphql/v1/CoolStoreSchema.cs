using GraphQL;
using GraphQL.Types;

namespace VND.CoolStore.Services.GraphQL.v1
{
    public class CoolStoreSchema : Schema
    {
        public CoolStoreSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            Query = resolver.Resolve<CoolStoreQuery>();
            // Mutation = resolver.Resolve<CoolStoreMutation>();
        }
    }
}
