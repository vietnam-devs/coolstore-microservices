using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ReverseProxy.Abstractions;
using Microsoft.ReverseProxy.Service;
using N8T.Infrastructure.OTel;
using N8T.Infrastructure.Tye;

namespace WebApiGateway
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Config = config;
        }

        private IConfiguration Config { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddCors(options =>
            {
                options.AddPolicy("api", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            var isRunOnTye = Config.IsRunOnTye("inventoryapp");

            // inventory
            var inventoryUrl = isRunOnTye
                ? $"{Config.GetServiceUri("inventoryapp")?.AbsoluteUri}"
                : Config.GetValue<string>("Services:inventoryapp");

            var invRoute = new ProxyRoute
            {
                RouteId = "inv",
                ClusterId = "inv-svc-cluster",
                Match =
                {
                    Path = "/inv/{**catch-all}"
                },
                Transforms = new List<IDictionary<string, string>>()
            };

            invRoute.AddTransformXForwarded();
            invRoute.AddTransformPathRemovePrefix("/inv");

            var invCluster = new Cluster
            {
                Id = "inv-svc-cluster",
                Destinations =
                {
                    {
                        "inv-svc-cluster/destination1", new Destination
                        {
                            Address = inventoryUrl
                        }
                    }
                }
            };

            // product catalog
            var productCatalogUrl = isRunOnTye
                ? $"{Config.GetServiceUri("productcatalogapp")?.AbsoluteUri}"
                : Config.GetValue<string>("Services:productcatalogapp");

            var prodRoute = new ProxyRoute
            {
                RouteId = "prod",
                ClusterId = "prod-svc-cluster",
                Match =
                {
                    Path = "/prod/{**catch-all}"
                },
                Transforms = new List<IDictionary<string, string>>()
            };

            prodRoute.AddTransformXForwarded();
            prodRoute.AddTransformPathRemovePrefix("/prod");

            var prodCluster = new Cluster
            {
                Id = "prod-svc-cluster",
                Destinations =
                {
                    {
                        "prod-svc-cluster/destination1", new Destination
                        {
                            Address = productCatalogUrl
                        }
                    }
                }
            };

            // shopping cart
            var shoppingCartUrl = isRunOnTye
                ? $"{Config.GetServiceUri("shoppingcartapp")?.AbsoluteUri}"
                : Config.GetValue<string>("Services:shoppingcartapp");

            var cartRoute = new ProxyRoute
            {
                RouteId = "cart",
                ClusterId = "cart-svc-cluster",
                Match =
                {
                    Path = "/cart/{**catch-all}"
                },
                Transforms = new List<IDictionary<string, string>>()
            };

            cartRoute.AddTransformXForwarded();
            cartRoute.AddTransformPathRemovePrefix("/cart");

            var cartCluster = new Cluster
            {
                Id = "cart-svc-cluster",
                Destinations =
                {
                    {
                        "cart-svc-cluster/destination1", new Destination
                        {
                            Address = shoppingCartUrl
                        }
                    }
                }
            };


            // sale
            var saleUrl = isRunOnTye
                ? $"{Config.GetServiceUri("saleapp")?.AbsoluteUri}"
                : Config.GetValue<string>("Services:saleapp");

            var saleRoute = new ProxyRoute
            {
                RouteId = "sale",
                ClusterId = "sale-svc-cluster",
                Match =
                {
                    Path = "/sale/{**catch-all}"
                },
                Transforms = new List<IDictionary<string, string>>()
            };

            saleRoute.AddTransformXForwarded();
            saleRoute.AddTransformPathRemovePrefix("/sale");

            var saleCluster = new Cluster
            {
                Id = "sale-svc-cluster",
                Destinations =
                {
                    {
                        "sale-svc-cluster/destination1", new Destination
                        {
                            Address = saleUrl
                        }
                    }
                }
            };

            // configure
            var routes = new[]
            {
                invRoute,
                prodRoute,
                cartRoute,
                saleRoute
            };

            var clusters = new[]
            {
                invCluster,
                prodCluster,
                cartCluster,
                saleCluster
            };

            services.AddReverseProxy()
                .LoadFromMemory(routes, clusters);

            services.AddHealthChecks()
                .AddUrlGroup(new Uri(System.IO.Path.Combine(inventoryUrl, "healthz")),
                    name: "inventoryapp-check", tags: new[] { "inventoryapp" })
                .AddUrlGroup(new Uri(System.IO.Path.Combine(productCatalogUrl, "healthz")),
                    name: "productcatalogapp-check", tags: new[] { "productcatalogapp" })
                .AddUrlGroup(new Uri(System.IO.Path.Combine(shoppingCartUrl, "healthz")),
                    name: "shoppingcartapp-check", tags: new[] { "shoppingcartapp" })
                .AddUrlGroup(new Uri(System.IO.Path.Combine(saleUrl, "healthz")),
                    name: "saleapp-check", tags: new[] { "saleapp" });

            services.AddCustomOtelWithZipkin(Config,
                o =>
                {
                    o.Endpoint = isRunOnTye
                        ? new Uri($"http://{Config.GetServiceUri("zipkin")?.DnsSafeHost}:9411/api/v2/spans")
                        : o.Endpoint;
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("api");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthz",
                    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions {Predicate = _ => true});

                endpoints.MapHealthChecks("/liveness",
                    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                    {
                        Predicate = r => r.Name.Contains("self")
                    });

                endpoints.MapGet("/", async context =>
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync("<h3>WebApiGateway</h3>");
                    await context.Response.WriteAsync("<br>");
                    await context.Response.WriteAsync("<a href='/inv/info'>Inventory&nbsp;|&nbsp;</a>");
                    await context.Response.WriteAsync("<a href='/prod/info'>Product Catalog&nbsp;|&nbsp;</a>");
                    await context.Response.WriteAsync("<a href='/cart/info'>Shopping Cart&nbsp;|&nbsp;</a>");
                    await context.Response.WriteAsync("<a href='/sale/info'>Sale&nbsp;</a>");
                });

                endpoints.MapReverseProxy(proxyPipeline =>
                {
                    proxyPipeline.UseAffinitizedDestinationLookup();
                    proxyPipeline.UseProxyLoadBalancing();
                    proxyPipeline.UseRequestAffinitizer();
                });
            });
        }
    }
}
