using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using VND.CoolStore.Services.Review.Infrastructure.Db;
using VND.Fw.Infrastructure.AspNetCore.Miniservice;
using VND.Fw.Infrastructure.EfCore.SqlServer;

namespace VND.CoolStore.Services.Review
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      var assemblies = new HashSet<Assembly>
      {
        typeof(Startup).GetTypeInfo().Assembly,
        typeof(MiniServiceExtensions).GetTypeInfo().Assembly
      };

      var claimToScopeMap = new Dictionary<string, string>
      {
        {"access_review_api", "review_api_scope"}
      };

      var scopes = new Dictionary<string, string>
      {
        {"review_api_scope", "Review APIs"}
      };

      var serviceParams = new ServiceParams
      {
        {"assemblies", assemblies},
        {"audience", "api"},
        {"claimToScopeMap", claimToScopeMap},
        {"scopes", scopes}
      };

      services.AddScoped(sp => serviceParams);
      services.AddEfCoreSqlServer();
      services.AddMiniService<ReviewDbContext>();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseMiniService();
    }
  }
}
