using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DbUp;
using DbUp.Engine;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using Serilog;
using VND.CoolStore.ProductCatalog.DataContracts.Dto.V1;
using VND.CoolStore.Search.DataContracts.Dto.V1;

namespace VND.CoolStore.DbMigration
{
    class Program
    {
        private static IConfiguration _configuration;

        private enum ServiceName
        {
            ShoppingCart = 0,
            ProductCatalog = 1,
            Inventory = 2,
            Search = 3
        }

        static async Task Main(string[] args)
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

            for (var argIndex = 0; argIndex < args.Length; argIndex++)
            {
                if (IsArg(args[argIndex], "shoppingcart"))
                {
                    Log.Information("Run migration for Shopping Cart Service");
                    Run(ServiceName.ShoppingCart);
                }
                else if (IsArg(args[argIndex], "productcatalog"))
                {
                    Log.Information("Run migration for Product Catalog Service");
                    Run(ServiceName.ProductCatalog);
                }
                else if (IsArg(args[argIndex], "inventory"))
                {
                    Log.Information("Run migration for Inventory Service");
                    Run(ServiceName.Inventory);
                }
                else if(IsArg(args[argIndex], "search"))
                {
                    Log.Information("Seed data for Search Service");
                    await SeedSearchDbAsync(ServiceName.Search);
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"{args[argIndex]} not found.");
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

        private static async Task SeedSearchDbAsync(ServiceName serviceName)
        {
            var connString = _configuration.GetValue<string>("ConnectionStrings:search");
            Log.Information($"Connection to EslasticSearch at {connString}");

            var settings = new ConnectionSettings(new Uri(connString))
                .DefaultMappingFor<SearchProductModel>(i => i
                    .IndexName("product")
                )
                .PrettyJson();

            var client = new ElasticClient(settings);
            //var clusterState = await client.Cluster.StateAsync();
            //Log.Information($"Cluster info is {JsonConvert.SerializeObject(clusterState)}");

            try
            {
                var seedData = Path.GetFullPath("products.json", AppContext.BaseDirectory);
                Log.Information($"Seed data file at {seedData}");

                using StreamReader sr = new StreamReader(seedData);
                var readData = await sr.ReadToEndAsync();
                //Log.Information($"Read seed data with content is {readData}");

                var productModels = JsonConvert.DeserializeObject<List<CatalogProductDto>>(readData);
                var products = new List<SearchProductModel>();

                foreach (var prod in productModels)
                {
                    products.Add(new SearchProductModel
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Description = prod.Desc,
                        Price = prod.Price,
                        ImageUrl = prod.ImageUrl,
                        Category = new SearchCategoryModel
                        {
                            Id = prod.CategoryId,
                            Name = prod.CategoryName
                        },
                        Inventory = new SearchInventoryModel
                        {
                            Id = prod.InventoryId,
                            Description = prod.InventoryDescription,
                            Location = prod.InventoryLocation,
                            Website = prod.InventoryWebsite
                        }
                    });
                }

                //Log.Information($"Finish to transformation data into model");
                //Log.Information(JsonConvert.SerializeObject(products));

                for (var index = 0; index < products.Count; index++)
                {
                    var forDebug = await client.IndexDocumentAsync(products[index]);
                    Log.Debug($"Index response info: {JsonConvert.SerializeObject(forDebug)}");
                }

                Log.Information($"Finish to index data into ElasticSearch");

                var result = await client.SearchAsync<SearchProductModel>(s => s
                    .Query(q => q
                        .MatchAll())
                );
                Log.Information($"Number of items on product index: {result.Hits.Count}");
                //Log.Information($"Search all items: {JsonConvert.SerializeObject(result)}");
            }
            catch (FileNotFoundException ex)
            {
                Log.Error($"Got an error is {ex.Message}");
            }
        }

        private static bool IsArg(string candidate, string name)
        {
            return (name != null && candidate.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
