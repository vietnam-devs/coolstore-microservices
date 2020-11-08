using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using N8T.Infrastructure.App.Dtos;
using Nest;
using Polly;
using Polly.Retry;
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
            var productModels = JsonSerializer.Deserialize<List<FlatProductDto>>(readData);
            var products = productModels.Select(prod =>
                new ProductDto
                {
                    Id = prod.Id,
                    Name = prod.Name,
                    Price = prod.Price,
                    ImageUrl = prod.ImageUrl,
                    Description = prod.Description,
                    Category = new CategoryDto {Id = prod.CategoryId, Name = prod.CategoryName},
                    Inventory = new InventoryDto
                    {
                        Id = prod.InventoryId,
                        Location = prod.InventoryLocation,
                        Website = prod.InventoryWebsite,
                        Description = prod.InventoryDescription
                    }
                }).ToList();

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
            });
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async ValueTask ElastichSearchIndexingAsync(IConfiguration config, IEnumerable<ProductDto> products,
            CancellationToken cancellationToken)
        {
            var esUrl = config.GetValue("ElasticSearch:Url", "http://localhost:9200");
            _logger.LogInformation($"ElasticSearch Url: {esUrl}");

            var settings = new ConnectionSettings(new Uri(esUrl))
                .DefaultMappingFor<ProductDto>(i => i.IndexName("product"))
                .PrettyJson();

            var client = new ElasticClient(settings);

            try
            {
                _logger.LogInformation($"Index data to ElasticSearch...");

                foreach (var product in products)
                {
                    await client.IndexDocumentAsync(product, cancellationToken);
                }

                _logger.LogInformation($"Finished indexing data to ElasticSearch");

                var result = await client.SearchAsync<ProductDto>(s => s
                    .Query(q => q
                        .MatchAll()), cancellationToken);

                _logger.LogInformation($"Number of items on product index: {result.Hits.Count}");
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
            return Policy.Handle<Exception>().WaitAndRetryAsync(
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
}
