using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NetCoreKit.Infrastructure.AspNetCore.Miniservice;
using NetCoreKit.Infrastructure.EfCore.MySql;
using VND.CoolStore.Services.Review.Infrastructure.Db;

namespace VND.CoolStore.Services.Review
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMiniService<ReviewDbContext>(
        svc =>
        {
          svc.AddEfCoreMySqlDb();
          svc.AddExternalSystemHealthChecks();
        },
        (_, __) => { },
        () => new Dictionary<string, object>
        {
          [Constants.ClaimToScopeMap] = new Dictionary<string, string>
          {
            ["access_review_api"] = "review_api_scope"
          },
          [Constants.Scopes] = new Dictionary<string, string>
          {
            ["review_api_scope"] = "Review APIs"
          },
          [Constants.Audience] = "api"
        }
      );
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseMiniService();
    }
  }
}
