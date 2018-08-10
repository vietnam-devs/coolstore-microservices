using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using VND.FW.Infrastructure.AspNetCore.Miniservice.Options;
using VND.FW.Infrastructure.EfCore.Db;
using VND.FW.Infrastructure.EfCore.Extensions;

namespace VND.FW.Infrastructure.AspNetCore.Miniservice.ConfigureServices
{
  public class DatabaseConfigureService : IDbConfigureServices
  {
    public int Priority { get; } = 100;

    public void Configure<TDbContext>(IServiceCollection services) where TDbContext : DbContext
    {
      IdentityModelEventSource.ShowPII = true;

      var svcProvider = services.BuildServiceProvider();
      var config = svcProvider.GetRequiredService<IConfiguration>();
      var serviceParams = svcProvider.GetRequiredService<ServiceParams>();
      var extendOptionsBuilder = svcProvider.GetRequiredService<IExtendDbContextOptionsBuilder>();
      var connStringFactory = svcProvider.GetRequiredService<IDatabaseConnectionStringFactory>();

      //TODO: refactor it 
      var fisrtAssembly = (serviceParams["assemblies"] as HashSet<Assembly>).FirstOrDefault();

      services.AddOptions()
        .Configure<PersistenceOption>(config.GetSection("EfCore"));

      void OptionsBuilderAction(DbContextOptionsBuilder o)
      {
        extendOptionsBuilder.Extend(o, connStringFactory, fisrtAssembly.GetName().Name);
      }

      services.AddDbContextPool<TDbContext>(OptionsBuilderAction);
      services.AddSingleton<TDbContext>();
      services.AddSingleton<DbContext>(resolver => resolver.GetRequiredService<TDbContext>());
      services.AddEfCore();
    }
  }
}
