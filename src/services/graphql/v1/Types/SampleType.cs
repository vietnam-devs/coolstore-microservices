using GraphQL.Types;

namespace VND.CoolStore.Services.GraphQL.v1.Types
{
    public class Sample
    {
        public string Name { get; set; }
    }

    public class SampleType : ObjectGraphType<Sample>
    {
        public SampleType()
        {
            Name = "Sample";
            Field(x => x.Name);
        }
    }
}
