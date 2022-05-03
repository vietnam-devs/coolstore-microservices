using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace N8T.Infrastructure.SeriLog;

public static class Extensions
{
    public static async Task WithSeriLog(Func<ValueTask> func)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            await func.Invoke();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Unhandled exception");
        }
        finally
        {
            Log.Information("Shut down complete");
            Log.CloseAndFlush();
        }
    }

    public static void AddSerilog(this ConfigureHostBuilder hostBuilder, string appName)
    {
        hostBuilder.UseSerilog((ctx, lc) => lc
            .WriteTo.Console()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", appName)
            .ReadFrom.Configuration(ctx.Configuration));
    }
}
