# TODO
1. error when do checkout, double-check for saleapp (we might add Azure Service Bus for pubsub)
2. scale rule with http concurrency (10)
3. Traffic spliting (modify web to add rating and split traffic 50/50)

# APIs

## Inventory (Rust)
### Internal HTTP (Dapr)

- POST: get-inventories-by-ids (calls from `product-catalog`)
  - Request: InventoryByIdsRequest, GetAvailabilityInventoriesQuery 
  - Response: IEnumerable<InventoryDto>

- POST: get-inventory-by-id (calls from `product-catalog`)
  - Request: InventoryRequest, GetInventoryQuery 
  - Response: InventoryDto

## Product Catalog (Rust)
### Public HTTP

- GET: api/product-search/{query}/{price}/{page}/{pageSize}
  - Request: SearchProductsQuery 
  - Response: SearchProductsResponse

- GET: api/products/{page}/{price}
  - Request: GetProductsByPriceAndNameQuery 
  - Response: IEnumerable<FlatProductDto>

- GET: api/products/{id}
  - Request: GetDetailOfSpecificProductQuery 
  - Response: FlatProductDto

### Internal HTTP (Dapr)

- POST: get-products-by-ids (not found)
  - Request: ProductByIdsRequest, GetProductsByIdsQuery 
  - Response: IEnumerable<ProductDto>

- POST: get-product-by-id (calls from `shopping-cart`)
  - Request: ProductByIdRequest, GetProductsByIdsQuery 
  - Response: ProductDto

## Shopping Cart (.NET 6)
### Public HTTP

- GET: api/carts
  - Request: GetCartByUserIdQuery
  - Response: CartDto
- POST: api/carts
  - Request: CreateShoppingCartWithProductCommand
  - Response: CartDto
- PUT: api/carts
  - Request: UpdateAmountOfProductInShoppingCartCommand
  - Response: CartDto
- PUT: api/carts/checkout
  - Request: CheckOutCommand
  - Response: CartDto

## Sale (Golang)
### Public HTTP

- GET: api/orders
  - Request: GetOrderListByUserQuery
  - Response: IEnumerable<OrderDto>

- POST: api/update-order-status
  - Request: OrderWithStatusRequest, UpdateOrderStatusQuery (TODO: Command)
  - bool

### Internal Subscriber

- POST: processing-order (subscribe event from `shopping-cart`)
  - Request: ShoppingCartCheckedOut (event), CreateOrderQuery (TODO: Command)

### Internal Cron Job (TODO: Refactor to ServerlessWorkflow)

- POST: cron-process-order
  - Request: ProcessOrderQuery (TODO: Command)
- POST: cron-complete-order
  - Request: CompleteOrderQuery (TODO: Command)

# Get starting

## docker-compose

- Because dev cert isn't working in docker-compose env, so we need to generate the cert just like https://github.com/thangchung/Sample-Docker-Https, and maps it into docker-compose volume

You need to copy the cert at `src/dotnet/certs/localhost.pfx` into `~/.aspnet/https` so that you can map the cert in local path into docker path when run `docker compose -f docker-compose.yml -f docker-compose.override.yml up -d`

See more about this issue at https://github.com/MicrosoftDocs/visualstudio-docs/issues/5733

## tye

```bash
> dotnet dev-certs https -ep aspnetapp.pfx -p P@ssw0rd # then save it to %USERPROFILE%\.aspnet\https
> dotnet dev-certs https --trust
> tye run
```

## Azure Container Apps with Bicep

```powershell
> $rgName="coolstore-rg"
> $location="eastus"
```

```powershell
> az group create -n $rgName -l $location
```

```powershell
> az deployment group create `
  --resource-group $rgName `
  --template-file main.bicep `
  --parameters  postgresDbPassword='<postgres password>'
```

```powershell
> az containerapp update `
  --name webapigatewayapp `
  --resource-group $rgName `
  --set-env-vars 'OpenIdConnect__Authority=https://<identityapp url>' 'ReverseProxy__Clusters__appCluster__Destinations__destination1__Address=https://<webapp url>'
```

```powershell
> az group delete -n $rgName
```
