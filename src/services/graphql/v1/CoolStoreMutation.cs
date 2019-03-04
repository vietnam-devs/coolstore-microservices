using GraphQL.Types;

namespace VND.CoolStore.Services.GraphQL.v1
{
    public class CoolStoreMutation : ObjectGraphType<object>
    {
        public CoolStoreMutation()
        {
            Name = "Mutation";
        }
    }
}
