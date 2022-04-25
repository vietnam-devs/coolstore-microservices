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


## ASP.NET Core Cert with docker-compose

```bash
> dotnet dev-certs https -ep aspnetapp.pfx -p P@ssw0rd # then save it to %USERPROFILE%\.aspnet\https
> dotnet dev-certs https --trust
> docker compose up
```