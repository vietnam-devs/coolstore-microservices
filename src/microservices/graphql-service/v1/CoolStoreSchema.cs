using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using tanka.graphql;
using tanka.graphql.error;
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
        private static IHttpContextAccessor _httpContext;

        public CoolStoreSchema(IHttpContextAccessor httpContext, ICoolStoreResolverService resolverService)
        {
            _httpContext = httpContext;
            var schema = IdlSchema.CreateAsync().Result;
            var schemaBuilder = new SchemaBuilder(schema);
            var resolvers = new CoolStoreResolvers(resolverService);

            CoolStore = SchemaTools.MakeExecutableSchemaWithIntrospection(
                schemaBuilder,
                resolvers,
                resolvers, new Dictionary<string, CreateDirectiveVisitor>
                {
                    ["authorize"] = AuthorizeVisitor()
                });
        }

        public ISchema CoolStore { get; set; }

        public static CreateDirectiveVisitor AuthorizeVisitor()
        {
            return builder => new DirectiveVisitor
            {
                FieldDefinition = (directive, fieldDefinition) =>
                {
                    return fieldDefinition.WithResolver(resolver => resolver.Use((context, next) =>
                    {
                        var user = _httpContext.HttpContext.User;
                        if (!user.Identity.IsAuthenticated)
                        {
                            throw new GraphQLError("Provide a valid bearer token in the header.");
                        }
                        return next(context);
                    }).Run(fieldDefinition.Resolver));
                }
            };
        }
    }
}
