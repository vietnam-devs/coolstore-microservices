using ShoppingCart.Core.Dtos;
using ShoppingCart.Core.Gateways;

namespace ShoppingCart.Infrastructure.Gateways;

public class ProductCatalogGateway : IProductCatalogGateway
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<ProductCatalogGateway> _logger;

    public ProductCatalogGateway(DaprClient daprClient, ILogger<ProductCatalogGateway> logger)
    {
        _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("{Prefix}: GetProductByIdAsync by id={Id}", nameof(ProductCatalogGateway), id);

        var product = await _daprClient.GetStateAsync<ProductDto>("statestore", $"product-{id}", cancellationToken: cancellationToken);
        if (product is not null) return product;

        var requestData = new ProductByIdRequest {Id = id};
        product = await _daprClient.InvokeMethodAsync<ProductByIdRequest, ProductDto>(
            "productcatalogapp", "get-product-by-id", requestData, cancellationToken: cancellationToken);

        if (product is null)
        {
            throw new CoreException($"Couldn't find out product with id={id}");
        }

        await _daprClient.SaveStateAsync("statestore", $"product-{id}", product, cancellationToken: cancellationToken);

        return product;
    }
}
