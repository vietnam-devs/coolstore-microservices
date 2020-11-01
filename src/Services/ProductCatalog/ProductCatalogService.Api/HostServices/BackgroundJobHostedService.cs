using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using ProductCatalogService.Application.Common;
using ProductCatalogService.Application.SearchProducts;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Policy = Polly.Policy;

namespace ProductCatalogService.Api.HostServices
{
    public class BackgroundJobHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger<BackgroundJobHostedService> _logger;

        public BackgroundJobHostedService(IServiceProvider serviceProvider,
            IHostApplicationLifetime appLifetime,
            ILogger<BackgroundJobHostedService> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _appLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var seedData = Path.GetFullPath("products.json", AppContext.BaseDirectory);
            _logger.LogInformation($"Seed data file at {seedData}");

            using StreamReader sr = new StreamReader(seedData);
            var readData = await sr.ReadToEndAsync();
            //_logger.LogInformation($"Read seed data with content is {readData}");

            // IMPORTANT: data should come from the database, we do this way for demo only 
            var productModels = JsonConvert.DeserializeObject<List<ProductDto>>(readData);
            var products = productModels.Select(prod =>
                new SearchProductModel(prod.Id, prod.Name, prod.Price, prod.ImageUrl, prod.Description,
                    new SearchCategoryModel(prod.CategoryId, prod.CategoryName),
                    new SearchInventoryModel(prod.InventoryId.Value, prod.InventoryLocation, prod.InventoryWebsite,
                        prod.InventoryDescription))).ToList();

            _appLifetime.ApplicationStarted.Register(async () =>
            {
                _logger.LogInformation("App started.");

                var policy = CreatePolicy(3, _logger, nameof(BackgroundJobHostedService));

                // indexing to ES
                await policy.ExecuteAsync(async () =>
                {
                    using var scope = _serviceProvider.CreateScope();

                    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                    await ElastichSearchIndexingAsync(config, products, cancellationToken);
                });

                // replication data to Dapr State
                await policy.ExecuteAsync(async () =>
                {
                    using var scope = _serviceProvider.CreateScope();

                    var daprClient = scope.ServiceProvider.GetRequiredService<DaprClient>();

                    await DaprStateReplicationAsync(daprClient, products, cancellationToken);
                });
            });
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async ValueTask DaprStateReplicationAsync(DaprClient daprClient,
            IEnumerable<SearchProductModel> products,
            CancellationToken cancellationToken)
        {
            if (daprClient is null)
            {
                throw new Exception("Couldn't get DaprClient from scope.");
            }

            //TODO: will remove it
            await daprClient.SaveStateAsync("statestore", "products", new ProductsState(products),
                cancellationToken: cancellationToken);

            await daprClient.PublishEventAsync("pubsub", "products-sync", new ProductsState(products), cancellationToken);

            _logger.LogInformation($"Put all products to dapr state completed.");
        }

        private async ValueTask ElastichSearchIndexingAsync(IConfiguration config, IEnumerable<SearchProductModel> products,
            CancellationToken cancellationToken)
        {
            var esUrl = config.GetValue("ElasticSearch:Url", "http://localhost:9200");
            _logger.LogInformation($"ElasticSearch Url: {esUrl}");

            var settings = new ConnectionSettings(new Uri(esUrl))
                .DefaultMappingFor<SearchProductModel>(i => i
                    .IndexName("product")
                )
                .PrettyJson();

            var client = new ElasticClient(settings);
            var clusterState = await client.Cluster.StateAsync(ct: cancellationToken);
            _logger.LogInformation($"Cluster info is {JsonConvert.SerializeObject(clusterState)}");

            try
            {
                //_logger.LogInformation($"Finish to transformation data into model");
                //_logger.LogInformation(JsonConvert.SerializeObject(products));
                foreach (var product in products)
                {
                    var forDebug = await client.IndexDocumentAsync(product, cancellationToken);
                    _logger.LogDebug($"Index response info: {JsonConvert.SerializeObject(forDebug)}");
                }

                _logger.LogInformation($"Finish to index data into ElasticSearch");

                var result = await client.SearchAsync<SearchProductModel>(s => s
                    .Query(q => q
                        .MatchAll()), cancellationToken);

                _logger.LogInformation($"Number of items on product index: {result.Hits.Count}");
                //_logger.LogInformation($"Search all items: {JsonConvert.SerializeObject(result)}");
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError($"Got an error is {ex.Message}");
            }
            finally
            {
                _logger.LogInformation("Done indexing data to ES.");
            }
        }

        private static AsyncRetryPolicy CreatePolicy(int retries, ILogger logger, string prefix)
        {
            return Policy.Handle<SqlException>().WaitAndRetryAsync(
                retries,
                retry => TimeSpan.FromSeconds(5),
                (exception, timeSpan, retry, ctx) =>
                {
                    logger.LogWarning(exception,
                        "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}",
                        prefix, exception.GetType().Name, exception.Message, retry, retries);
                }
            );
        }
    }

    //TODO; refactor with event & integration event
    public record ProductsState(IEnumerable<SearchProductModel> Products);
}
