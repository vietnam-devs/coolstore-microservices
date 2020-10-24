using Google.Protobuf;
using HotChocolate.Types;

namespace N8T.Infrastructure.GraphQL
{
    public class ProtoObjectType<T> : ObjectType<T>
        where T : IMessage<T>
    {
        protected override void Configure(IObjectTypeDescriptor<T> descriptor)
        {
            descriptor.Field(t => t.CalculateSize()).Ignore();
            descriptor.Field(t => t.Clone()).Ignore();
            descriptor.Field(t => t.Equals(default)).Ignore();
        }
    }
}
