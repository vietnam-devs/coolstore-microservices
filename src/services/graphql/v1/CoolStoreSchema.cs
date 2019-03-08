using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using tanka.graphql;
using tanka.graphql.sdl;
using tanka.graphql.tools;
using tanka.graphql.type;

namespace VND.CoolStore.Services.GraphQL.v1
{
    public static class IdlSchema
    {
        public static async Task<ISchema> CreateAsync()
        {
            var idl = await LoadIdlFromResourcesAsync();
            var schema = Sdl.Schema(Parser.ParseDocument(idl));

            return schema;
        }

        private static async Task<string> LoadIdlFromResourcesAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceStream =
                assembly.GetManifestResourceStream("VND.CoolStore.Services.GraphQL.v1.coolstore.graphql");

            using (var reader =
                new StreamReader(resourceStream ?? throw new InvalidOperationException(), Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }

    public class CoolStoreSchema
    {
        public CoolStoreSchema(ICoolStoreResolverService resolverService)
        {
            var schema = FromIdlAsync().Result;
            var resolvers = new CoolStoreResolvers(resolverService);

            CoolStore = SchemaTools.MakeExecutableSchemaWithIntrospection(
                schema,
                resolvers
                ).Result;
        }

        public Task<ISchema> FromIdlAsync()
        {
            return IdlSchema.CreateAsync();
        }

        public ISchema CoolStore { get; set; }
    }
}
