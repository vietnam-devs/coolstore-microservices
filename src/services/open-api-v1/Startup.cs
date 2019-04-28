using System;
using System.Linq;
using System.Text.RegularExpressions;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using VND.CoolStore.Services.OpenApiV1.Swagger;
using static Coolstore.CatalogService;
using static Coolstore.RatingService;
using static VND.CoolStore.Services.Cart.v1.Grpc.CartService;
using static VND.CoolStore.Services.Inventory.v1.Grpc.InventoryService;

namespace VND.CoolStore.Services.OpenApiV1
{
    public class Startup
    {
        public static readonly SymmetricSecurityKey
            SecurityKey = new SymmetricSecurityKey(Guid.NewGuid().ToByteArray());

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            IdentityModelEventSource.ShowPII = true;
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppOptions>(Configuration);
            RegisterApiVersioning(services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            RegisterDependencyServices(services);
            RegisterGrpcServices(services);
            RegisterOpenApi(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var basePath = Configuration["Hosts:BasePath"];
            basePath = basePath.EndsWith('/') ? basePath.TrimEnd('/') : basePath;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UsePathBase(basePath);

            app.UseHsts();
            app.UseCors("CorsPolicy");
            app.UseStaticFiles();

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(
                c =>
                {
                    var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var description in provider.ApiVersionDescriptions)
                        c.SwaggerEndpoint(
                            $"{basePath}/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                });
        }

        private void RegisterApiVersioning(IServiceCollection services)
        {
            services.AddRouting(o => o.LowercaseUrls = true);
            services
                .AddVersionedApiExplorer(
                    o =>
                    {
                        o.GroupNameFormat = "'v'VVV";
                        o.SubstituteApiVersionInUrl = true;
                    });

            services
                .AddMvcCore()
                .AddJsonFormatters(o => o.ContractResolver = new CamelCasePropertyNamesContractResolver())
                .AddDataAnnotations();

            services
                .AddApiVersioning(o =>
                {
                    o.ReportApiVersions = true;
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    o.DefaultApiVersion = ParseApiVersion(Configuration.GetValue<string>("ApiVersion"));
                });
        }

        private void RegisterOpenApi(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                c.DescribeAllEnumsAsStrings();
                foreach (var description in provider.ApiVersionDescriptions)
                    c.SwaggerDoc(
                        description.GroupName,
                        CreateInfoForApiVersion(Configuration, description));

                // c.IncludeXmlComments (XmlCommentsFilePath);

                c.EnableAnnotations();
                c.OperationFilter<DefaultValuesOperationFilter>();
                c.SchemaFilter<SwaggerExcludeSchemaFilter>();
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            Info CreateInfoForApiVersion(IConfiguration config, ApiVersionDescription description)
            {
                var info = new Info
                {
                    Title = "OpenApi Service v1",
                    Version = "v1.0",
                    Description = "OpenApi Service"
                };

                if (description.IsDeprecated)
                    info.Description += " This API version has been deprecated.";

                return info;
            }
        }

        private static void RegisterDependencyServices(IServiceCollection services)
        {
        }

        private void RegisterGrpcServices(IServiceCollection services)
        {
            services.AddSingleton(
                typeof(CartServiceClient),
                RegisterGrpcService<CartServiceClient>("CartEndPoint"));

            services.AddSingleton(
                typeof(CatalogServiceClient),
                RegisterGrpcService<CatalogServiceClient>("CatalogEndPoint"));

            services.AddSingleton(
                typeof(InventoryServiceClient),
                RegisterGrpcService<InventoryServiceClient>("InventoryEndPoint"));

            services.AddSingleton(
                typeof(RatingServiceClient),
                RegisterGrpcService<RatingServiceClient>("RatingEndPoint"));
        }

        private static ApiVersion ParseApiVersion(string serviceVersion)
        {
            if (string.IsNullOrEmpty(serviceVersion))
                throw new Exception("ServiceVersion is null or empty.");

            const string pattern = @"(.)|(-)";
            var results = Regex.Split(serviceVersion, pattern)
                .Where(x => x != string.Empty && x != "." && x != "-")
                .ToArray();

            if (results == null || results.Count() < 2)
                throw new Exception("Could not parse ServiceVersion.");

            if (results.Count() > 2)
                return new ApiVersion(
                    Convert.ToInt32(results[0]),
                    Convert.ToInt32(results[1]),
                    results[2]);

            return new ApiVersion(
                Convert.ToInt32(results[0]),
                Convert.ToInt32(results[1]));
        }

        private TService RegisterGrpcService<TService>(string serviceName)
            where TService : ClientBase<TService>
        {
            var rpcClients = Configuration.GetSection("GrpcEndPoints");
            var channel = new Channel(rpcClients[serviceName], ChannelCredentials.Insecure);
            var client = (TService)typeof(TService)
                .GetConstructor(new[] { typeof(Channel) })
                .Invoke(new object[] { channel });

            return client;
        }
    }
}
