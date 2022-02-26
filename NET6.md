# APIs

## Inventory (Rust)
### Internal HTTP (Dapr)

- POST: get-inventories-by-ids
  - Request: InventoryByIdsRequest, GetAvailabilityInventoriesQuery 
  - Response: IEnumerable<InventoryDto>

- POST: get-inventory-by-id
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

- POST: get-products-by-ids
  - Request: ProductByIdsRequest, GetProductsByIdsQuery 
  - Response: IEnumerable<ProductDto>

- POST: get-product-by-id
  - Request: ProductByIdRequest, GetProductsByIdsQuery 
  - Response: ProductDto

## Shopping Cart (.NET 6)
### Public HTTP

- GET: api/carts
  - Request: GetCartByUserIdQuery
  - Response: CartDto
- POST: api/carts
  - Request: CreateShoppingCartWithProductQuery (TODO: Command)
  - Response: CartDto
- PUT: api/carts
  - Request: UpdateAmountOfProductInShoppingCartQuery (TODO: Command)
  - Response: CartDto
- PUT: api/carts/checkout
  - Request: CheckOutQuery (TODO: Command)
  - Response: CartDto

## Sale (Golang)
### Public HTTP

- GET: api/orders
  - Request: GetOrderListByUserQuery
  - Response: IEnumerable<OrderDto>

### Internal HTTP 

- POST: update-order-status
  - Request: OrderWithStatusRequest, UpdateOrderStatusQuery (TODO: Command)
  - bool

### Internal Subscriber

- POST: processing-order
  - Request: ShoppingCartCheckedOut (event), CreateOrderQuery (TODO: Command)

### Internal Cron Job (TODO: Refactor to ServerlessWorkflow)

- POST: cron-process-order
  - Request: ProcessOrderQuery (TODO: Command)
- POST: cron-complete-order
  - Request: CompleteOrderQuery (TODO: Command)