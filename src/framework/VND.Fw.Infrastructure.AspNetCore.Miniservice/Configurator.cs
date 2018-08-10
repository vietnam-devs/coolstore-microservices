using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VND.Fw.Infrastructure.AspNetCore.Miniservice
{
  public interface IPriorityConfigure
  {
    int Priority { get; }
  }

  public interface IBasicConfigureServices : IPriorityConfigure
  {
    void Configure(IServiceCollection services);
  }

  public interface IDbConfigureServices : IPriorityConfigure
  {
    void Configure<TDbContext>(IServiceCollection services) where TDbContext : DbContext;
  }

  public interface IConfigureApplication : IPriorityConfigure
  {
    void Configure(IApplicationBuilder appBuilder);
  }
}
