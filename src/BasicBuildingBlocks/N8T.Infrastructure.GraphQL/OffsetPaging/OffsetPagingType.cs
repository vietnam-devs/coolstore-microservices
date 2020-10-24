using HotChocolate.Types;

namespace N8T.Infrastructure.GraphQL.OffsetPaging
{
    public class OffsetPagingType<TSchemaType, TValueType> : ObjectType<OffsetPaging<TValueType>>
        where TSchemaType : class, IOutputType
        where TValueType : new()
    {
        protected override void Configure(IObjectTypeDescriptor<OffsetPaging<TValueType>> descriptor)
        {
            descriptor.Field(t => t.Edges)
                .Name("edges")
                .Description("A list of edges.")
                .Type<ListType<TSchemaType>>();

            descriptor.Field(t => t.TotalCount)
                .Type<NonNullType<IntType>>();
        }
    }
}