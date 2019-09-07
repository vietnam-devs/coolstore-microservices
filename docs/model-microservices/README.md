# Business Scenario and Analysis

CoolStore Website has the basic business scenario for Product Catalog, Shopping Cart, Payment Process, Inventory, Rating, and Access Control.

## Business Scenario

### Product Catalog

- As a Buyer, I want to see the list of products with filtering, sorting on the home page (name, photo, short description, rating, and hot product flag which is a product with a lot of people see or buy).
  - Whenever filtering with any price and name of the product, then the list of products need to narrow down with appropriate products.
  - Whenever sorting with descending or ascending on the price or name of the product, then the list of products need to follow with this sorting.
  - Whenever both filtering and sorting in action, then the list of products shall be effective by both of description above.
- As a Buyer, I want to navigate into the detail of one product with the basic attributes such as name, description, available product in the inventory, the inventory store information like stock address and location, a hot product flag (if has) and rating.
- As a SysAdmin, I want to manage a product (CRUD actions) and assign one existing inventory into the product.

### Shopping Cart

- As a Buyer, I want to buy any product on the product catalog page (add this product into the shopping cart - one product will be added by default).
- As a Buyer, I want to see the product detail and buy this product if I like (add this product into the shopping cart - one product will be added by default).
- As a Buyer, I want to see the list of products I just added into the shopping cart, and I would like to see the summary information panel for the current shopping cart such as cart total cost, promotion item saving cost, subtotal cost, shipping cost, promotion shipping savings cost, total order amount on this page.
- As a Buyer, I want to update the amount of product in the shopping cart.
  - Whenever updating amount of product happens, then the summary information panel needs to be updated accordingly to changes.
- As a Buyer, I want to delete any product in the shopping cart which they don't want to buy anymore.
  - Whenever the deleting amount of product happens, then the summary information panel needs to be updated accordingly to changes.
- As a Buyer, I want to do check out my shopping cart.
  - Whenever the number of products in the shopping cart is zero then this checks out process does not happen.
- While a shopping cart was checked out, the payment process starts.
- As a SysAdmin, I want to see shopping cart of all buyers with information about cart total cost, promotion item saving cost, subtotal cost, shipping cost, promotion shipping savings cost, total order amount.
- As a SysAdmin, I want to enable/disable any shopping cart of any buyer.

### Payment Process

- Any Buyer can do the payment.
- In the moment of the payment process happening, we start to validation the item information, process for payment and subsequently send an email to the Buyer (because this is just the demo so we don't actually integrate with the payment gateway)
  - Whenever any product information is invalid, then the payment process will be canceled and one email will be sent to Buyer for notification.
  - Whenever ending this payment process, we mark the payment of this cart is processed status and send an email to let Buyer knows.

### Inventory

- As a SysAdmin, I want to manage inventory (CRUD actions).

### Rating

- As a Buyer, I want to rate for each product that I think is good (1 -> 5 stars).

### Access Control

- Each Buyer/SysAdmin is a User.
- As a Buyer/SysAdmin, I want to log-in to the system.
  - Whenever a user with a Buyer role does a login, then I will be brought to the product catalog page.
  - Whenever a user with a SysAdmin role does a login, then I will be brought to the administration page.
- As a Buyer/SysAdmin, I want to log-out of the system.

### One-off tasks

- Seeding the sample data for the product.
- Seeding the sample data for inventory.
- Seeding the sample data for the rating of a few products (randomness).
- Seeding a Bob user with SysAdmin role.
- Seeding a Mary and Alice users with Buyer role.

## Conceptual Model

![](/coolstore-microservices/conceptual-model.png)

## Fine Tuning 1: Finding important statements starts with verbs

### Product Catalog

- As a **Buyer**, I want to **see the list of products with filtering, sorting on the home page** (name, photo, short description, rating, and hot product flag which is a product with a lot of people see or buy).
  - Whenever filtering with any price and name of the product, then the list of products need to narrow down with appropriate products.
  - Whenever sorting with descending or ascending on the price or name of the product, then the list of products need to follow with this sorting.
  - Whenever both filtering and sorting in action, then the list of products shall be effective by both of description above.
- As a **Buyer**, I want to **navigate into the detail of one product** with the basic attributes such as name, description, available product in the inventory, the inventory store information like stock address and location, a hot product flag (if has) and rating.
- As a **SysAdmin**, I want to **manage a product** (CRUD actions) and **assign one existing inventory into the product**.

### Shopping Cart

- As a **Buyer**, I want to **buy any product on the product catalog page** (add this product into the shopping cart - one product will be added by default).
- As a **Buyer**, I want to **see the product detail and buy this product** if I like (add this product into the shopping cart - one product will be added by default).
- As a **Buyer**, I want to **see the list of products** I just added into the shopping cart, and I would like to **see the summary information panel** for the current shopping cart such as cart total cost, promotion item saving cost, subtotal cost, shipping cost, promotion shipping savings cost, total order amount on this page.
- As a **Buyer**, I want to **update the amount of product in the shopping cart**.
  - Whenever updating amount of product happens, then the summary information panel needs to be updated accordingly to changes.
- As a **Buyer**, I want to **delete any product in the shopping cart** which they don't want to buy anymore.
  - Whenever the deleting amount of product happens, then the summary information panel needs to be updated accordingly to changes.
- As a **Buyer**, I want to do **check out my shopping cart**.
  - Whenever the number of products in the shopping cart is zero then this checks out process does not happen.
- While a shopping cart was checked out, the payment process starts.
- As a **SysAdmin**, I want to **see shopping cart of all buyers with information** about cart total cost, promotion item saving cost, subtotal cost, shipping cost, promotion shipping savings cost, total order amount.
- As a **SysAdmin**, I want to **enable/disable any shopping cart of any buyer**.

### Payment Process

- Any Buyer can do the payment.
- In the moment of the **payment process** happening, we start to **validation the item information**, **process for payment** and subsequently **send an email to the Buyer** (because this is just the demo so we don't actually integrate with the payment gateway)
  - Whenever any product information is invalid, then the payment process will be canceled and one email will be sent to Buyer for notification.
  - Whenever ending this payment process, we mark the payment of this cart is processed status and send an email to let Buyer knows.

### Inventory

- As a **SysAdmin**, I want to **manage inventory** (CRUD actions).

### Rating

- As a **Buyer**, I want to **rate for each product** that I think is good (1 -> 5 stars).

### Access Control

- Each Buyer/SysAdmin is a User.
- As a **Buyer/SysAdmin**, I want to **log-in to the system**.
  - Whenever a user with a Buyer role does a login, then I will be brought to the product catalog page.
  - Whenever a user with a SysAdmin role does a login, then I will be brought to the administration page.
- As a **Buyer/SysAdmin**, I want to **log-out of the system**.

## Fine tuning 2: Get rid of un-important statements

### Product Catalog

- [Buyer] see the list of products with filtering and sorting on price and name -> get products by filtering and sorting on price and name
- [Buyer] navigate into the detail of one product -> get the detail of specific product
- [SysAdmin] manage product -> create, update and delete a product
- [SysAdmin] assign one existing inventory into the product -> update product with inventory

### Shopping Cart

- [Buyer] buy any product on the product catalog page -> create the shopping cart with product
- [Buyer] see the list of products -> get shopping cart with products
- [Buyer] see the summary information panel (at the client side only)
- [Buyer] update the amount of product in the shopping cart
- [Buyer] delete any product in the shopping cart -> delete product in the shopping cart
- [Buyer] check out my shopping cart -> check out the shopping cart
- [SysAdmin] see shopping cart of all buyers with information -> get shopping cart of buyers
- [SysAdmin] enable/disable any shopping cart of any buyer -> update enabled/disabled shopping cart of buyer

### Payment Process

- [System] validation the item information
- [System] process for payment
- [System] send an email to the Buyer

### Inventory

- [SysAdmin] manage inventory -> view, create, update and delete an inventory

### Rating

- [Buyer] rate for each product -> create rating for product

### Access Control

- [User] log in into the system
- [User] log out of the system

## Usecase View

### Product Catalog

![](/coolstore-microservices/usecases-product-catalog.png)

### Shopping Cart

![](/coolstore-microservices/usecases-shopping-cart.png)

### Payment Process

![](/coolstore-microservices/usecases-payment-process.png)

### Inventory

![](/coolstore-microservices/usecases-inventory.png)

### Rating

![](/coolstore-microservices/usecases-rating.png)

### Access Control

![](/coolstore-microservices/usecases-access-control.png)

## Big Picture Event Storming

### Product Catalog

- [Buyer] get products by filtering and sorting on price and name
  - ProductListDisplayed (filter + sorting)
- [Buyer] get the detail of specific product
  - ProductDisplayed
- [SysAdmin] product management
  - ProductListDisplayed
  - ProductCreated
  - ProductUpdated
  - ProductDeleted
- [SysAdmin] update product with inventory
  - InventoryAssigned

### Shopping Cart

- [Buyer] create the shopping cart with product
  - ShoppingCartWithProductCreated
- [Buyer] get shopping cart with products
  - ShoppingCartWithProductsDisplayed
- [Buyer] update the amount of product in the shopping cart
  - AmountOfProductInShoppingCartUpdated
- [Buyer] delete product in the shopping cart
  - ProductInShoppingCartDeleted
- [Buyer] check out the shopping cart
  - ShoppingCartCheckedOut
- [SysAdmin] get shopping cart of buyers
  - BuyersShoppingCartDisplayed
- [SysAdmin] update enabled/disabled shopping cart of buyer
  - ShoppingCartEnabled
  - ShoppingCartDisabled

### Payment Process

- [System]
  - ProductsValidated
  - PaymentProcessed
  - EmailSent

### Inventory

- [SysAdmin] inventory management
  - InventoryListDisplayed
  - InventoryCreated
  - InventoryUpdated
  - InventoryDeleted

### Rating

- [Buyer] create rating for product
  - ProductRated

### Access Control

- [User] log in into the system
  - UserLoggedOn
- [User] log out of the system
  - UserLoggedOut

![](/coolstore-microservices/eventstorming-big-picture.png)

## Roles, Commands and Events mapping

TODO

## Bounded Context forming up

TODO

## Context Maps

TODO
