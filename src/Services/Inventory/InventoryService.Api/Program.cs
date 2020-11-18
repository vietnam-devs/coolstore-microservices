using N8T.Infrastructure;
using N8T.Infrastructure.Helpers;

namespace InventoryService.Api
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var (hostBuilder, isRunOnTye) = HostHelper.CreateHostBuilder<Startup>(args);
            return hostBuilder.Run(isRunOnTye);
        }
    }
}
