using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using VND.Services.Inventory.v1.Service;
using VND.Services.Inventory.v1.Service.Impl;

namespace VND.Services.Inventory
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddMvcCore().AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddMvc();

            services.AddApiVersioning(o => o.ReportApiVersions = true);

            services.AddSwaggerGen(
                options =>
                {
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                    }

                    // options.IncludeXmlComments (XmlCommentsFilePath);
                });


        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            string basePath = Environment.GetEnvironmentVariable("ASPNETCORE_BASEPATH");
            if (!string.IsNullOrEmpty(basePath))
            {
                app.Use(async (context, next) =>
                {
                    context.Request.PathBase = basePath;
                    await next.Invoke();
                });
            }

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(
                options =>
                {
                    var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });
        }

        static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info()
            {
                Title = $"Sample API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "A sample application with Swagger, Swashbuckle, and API versioning.",
                Contact = new Contact() { Name = "VN Devs", Email = "vietnam.devs.group@gmail.com" },
                TermsOfService = "Shareware",
                License = new License() { Name = "MIT", Url = "https://opensource.org/licenses/MIT" }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
