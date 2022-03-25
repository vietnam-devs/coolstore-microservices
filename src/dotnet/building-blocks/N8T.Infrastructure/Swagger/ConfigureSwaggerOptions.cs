using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace N8T.Infrastructure.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.SwaggerDoc("APIs v1",
                new OpenApiInfo()
                {
                    Title = "APIs",
                    Version = "v1",
                    Description = "An application with Swagger, Swashbuckle, and API versioning.",
                    Contact = new OpenApiContact() { Name = "Thang Chung", Email = "thangchung.onthenet@gmail.com" },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
        }
    }
}
