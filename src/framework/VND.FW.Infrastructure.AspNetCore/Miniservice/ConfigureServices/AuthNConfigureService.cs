using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VND.FW.Infrastructure.AspNetCore.Extensions;

namespace VND.FW.Infrastructure.AspNetCore.Miniservice.ConfigureServices
{
  public class AuthNConfigureService : IBasicConfigureServices
  {
    public int Priority { get; } = 700;

    public void Configure(IServiceCollection services)
    {
      var svcProvider = services.BuildServiceProvider();
      var config = svcProvider.GetRequiredService<IConfiguration>();
      var env = svcProvider.GetRequiredService<IHostingEnvironment>();
      var serviceParams = svcProvider.GetRequiredService<ServiceParams>();

      if (config.GetValue("EnableAuthN", false))
      {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services
          .AddAuthentication(options =>
          {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
          })
          .AddJwtBearer(options =>
          {
            options.Authority = GetAuthUri(config, env);
            options.RequireHttpsMetadata = false;
            options.Audience = serviceParams["audience"].ToString();
          });

        services.AddAuthorization(c =>
        {
          if (serviceParams.TryGetValue("claimToScopeMap", out var claimToScopeMap))
          {
            foreach (var claimToScope in (IDictionary<string, string>)claimToScopeMap)
            {
              c.AddPolicy(claimToScope.Key, p => p.RequireClaim("scope", claimToScope.Value));
            }
          }
        });
      }
    }

    private static string GetAuthUri(IConfiguration config, IHostingEnvironment env)
    {
      return config.GetHostUri(env, "Auth");
    }
  }
}
