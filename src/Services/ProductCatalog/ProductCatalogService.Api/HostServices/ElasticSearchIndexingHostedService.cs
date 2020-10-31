using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using ProductCatalogService.Application.SearchProducts;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Policy = Polly.Policy;

namespace ProductCatalogService.Api.HostServices
{
    public class ElasticSearchIndexingHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ElasticSearchIndexingHostedService> _logger;

        public ElasticSearchIndexingHostedService(IServiceProvider serviceProvider,
            ILogger<ElasticSearchIndexingHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken); // waiting for elasticsearch up

            var policy = CreatePolicy(3, _logger, nameof(ElasticSearchIndexingHostedService));
            await policy.ExecuteAsync(async () =>
            {
                using var scope = _serviceProvider.CreateScope();

                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

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
                    var seedData = Path.GetFullPath("products.json", AppContext.BaseDirectory);
                    _logger.LogInformation($"Seed data file at {seedData}");

                    using StreamReader sr = new StreamReader(seedData);
                    var readData = await sr.ReadToEndAsync();
                    //_logger.LogInformation($"Read seed data with content is {readData}");

                    var productModels = JsonConvert.DeserializeObject<List<ProductDataModel>>(readData);
                    var products = productModels.Select(prod =>
                        new SearchProductModel(prod.Id, prod.Name, prod.Price, prod.ImageUrl, prod.Description,
                            new SearchCategoryModel(prod.CategoryId, prod.CategoryName),
                            new SearchInventoryModel(prod.InventoryId, prod.InventoryLocation, prod.InventoryWebsite,
                                prod.InventoryDescription))).ToList();

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

                _logger.LogInformation("Done indexing data to ES.");
            });
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

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

    public record ProductDataModel(Guid Id, string Name, double Price, string ImageUrl, string Description,
        Guid InventoryId, string InventoryLocation, string InventoryWebsite, string InventoryDescription,
        Guid CategoryId, string CategoryName);
}
