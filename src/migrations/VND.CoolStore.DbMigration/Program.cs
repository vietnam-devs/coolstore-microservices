using System;
using DbUp;
using DbUp.Engine;
using DbUp.SqlServer;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace VND.CoolStore.DbMigration
{
    class Program
    {
        private static IConfiguration _configuration;

        private enum ServiceName
        {
            ShoppingCart = 0,
            ProductCatalog = 1,
            Inventory = 2
        }

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var lastArg = 0;
            for (; lastArg < args.Length; lastArg++)
            {
                if (IsArg(args[lastArg], "shoppingcart"))
                {
                    Log.Information("Run migration for Shopping Cart Service");
                    Run(ServiceName.ShoppingCart);
                }
                if (IsArg(args[lastArg], "productcatalog"))
                {
                    Log.Information("Run migration for Product Catalog Service");
                    Run(ServiceName.ProductCatalog);
                }
                if (IsArg(args[lastArg], "inventory"))
                {
                    Log.Information("Run migration for Inventory Service");
                    Run(ServiceName.Inventory);
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"{args[lastArg]} not found.");
                }
            }
        }

        private static void Run(ServiceName dBName)
        {
            var connString = _configuration.GetConnectionString(dBName.ToString());
            var scriptFolderPath = $"./Scripts/{dBName.ToString()}";

            DropDatabase.For.SqlDatabase(connString);
            EnsureDatabase.For.SqlDatabase(connString);

            var upgrader = DeployChanges.To
                .SqlDatabase(connString, null)
                .WithScriptsFromFileSystem(scriptFolderPath, new SqlScriptOptions
                {
                    RunGroupOrder = DbUpDefaults.DefaultRunGroupOrder + 1
                })
                .LogToAutodetectedLog()
                .Build();

            upgrader.PerformUpgrade();
        }

        private static bool IsArg(string candidate, string name)
        {
            return (name != null && candidate.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
