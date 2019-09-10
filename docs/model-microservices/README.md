# Business Scenario and Analysis

## Business Context

CoolStore Website has the basic business scenario for **Product Catalog**, **Shopping Cart**, **Payment Process**, **Inventory**, **Rating**, and **Access Control**.

With **Product Catalog**, **Buyer** can browse the **products list** with supported filtering and sorting by product name and price functionalities. And she can see the **detail of the product** on the product list page by clicking on it, in the detail page, she can see a name, description, available product in the **inventory**, the inventory store information like stock address and location, a **hot product** flag (if has) and **rating**. **SysAdmin** in the system can **manage the product** and has the ability to **assign this product to the existed inventory**.

With **Shopping Cart**, **Buyer** can **buy** any **product** on the **product list** via the Buy button on any product. Besides, she can **buy** the product in the **detail product page**. After bought product, she should see these products in the shopping cart and the **summary panel** with basic information such as cart total cost, promotion item saving cost, subtotal cost, shipping cost, promotion shipping savings cost, total order amount. And whenever she **buy more products** or **remove some products** out of the cart, then the **summary panel** and **shopping cart** should be **updated**. After all, she can do a **checkout process** to pay money by clicking the Check Out button on the shopping cart page. **SysAdmin** can **see all the shopping cart of any user** so that he can **enable or disable** any invalid shopping cart in CoolStore website.

With **Payment process**, after Buyer clicked checkout button, the system will start to **validate the product** information, **process payment**, and then **send an email** to **Buyer** so that she knows about what is going on.

With **Inventory**, **SysAdmin** can **manage the inventory**.

With **Rating**, **Buyer** can **rate any product** that she thinks is good (1 -> 5 stars).

With **Access Control**, **Buyer** or **SysAdmin** can **log on/off the system** and if **Buyer**, she will be brought into the **product catalog page**, and if **SysAdmin**, he will be brought into the **administration page**.

Some of the **one-off tasks** need to be done when CoolStore website starts such as **SysAdmin user, two Buyer users** and **sample data** for **product**, **inventory**, **rating** for a **few products**.

## Conceptual Model

![](/coolstore-microservices/conceptual-model.png)

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

## Event Storming: Events

::: tip
Finding Events via Usecases
:::

### Product Catalog

- **Buyer** get products by filtering and sorting on price and name
  - ProductListDisplayed (filter + sorting)
- **Buyer** get the detail of specific product
  - ProductDisplayed
- **SysAdmin** product management
  - ProductListDisplayed
  - ProductCreated
  - ProductUpdated
  - ProductDeleted
- **SysAdmin** update product with inventory
  - InventoryAssigned

### Shopping Cart

- **Buyer** create the shopping cart with product
  - ShoppingCartWithProductCreated
- **Buyer** get shopping cart with products
  - ShoppingCartWithProductsDisplayed
- **Buyer** update the amount of product in the shopping cart
  - AmountOfProductInShoppingCartUpdated
- **Buyer** delete product in the shopping cart
  - ProductInShoppingCartDeleted
- **Buyer** check out the shopping cart
  - ShoppingCartCheckedOut
- **SysAdmin** get shopping cart of buyers
  - BuyersShoppingCartDisplayed
- **SysAdmin** update enabled/disabled shopping cart of buyer
  - ShoppingCartEnabled
  - ShoppingCartDisabled

### Payment Process

- **System**
  - ProductsValidated
  - PaymentProcessed
  - EmailSent

### Inventory

- **SysAdmin** inventory management
  - InventoryListDisplayed
  - InventoryCreated
  - InventoryUpdated
  - InventoryDeleted

### Rating

- **Buyer** create rating for product
  - ProductRated

### Access Control

- **User** log on the system
  - UserLoggedOn
- **User** log out of the system
  - UserLoggedOut

![](/coolstore-microservices/es-big-picture.png)

## Event Storming: Roles, Commands and Events mapping

![](/coolstore-microservices/es-role-event-command.png)

## Event Storming: Bounded Context forming up

![](/coolstore-microservices/es-bounded-context.png)

## Event Storming: Context Maps

![](/coolstore-microservices/es-context-map-1.png)

::: tip
Finding Integration Events via Events
:::

### Product Catalog

- ProductListDisplayed
- ProductDisplayed
- ProductListDisplayed
- ProductCreated
- ProductUpdated (**IntegrationEvent**)
- ProductDeleted (**IntegrationEvent**)
- InventoryAssigned

### Shopping Cart

- ShoppingCartWithProductCreated
- ShoppingCartWithProductsDisplayed
- AmountOfProductInShoppingCartUpdated
- ProductInShoppingCartDeleted
- ShoppingCartCheckedOut (**IntegrationEvent**)
- BuyersShoppingCartDisplayed
- ShoppingCartEnabled
- ShoppingCartDisabled

### Payment Process

- ProductsValidated
- PaymentProcessed
- EmailSent

### Inventory

- InventoryListDisplayed
- InventoryCreated
- InventoryUpdated (**IntegrationEvent**)
- InventoryDeleted (**IntegrationEvent**)

### Rating

- ProductRated (**IntegrationEvent**)

### Access Control

- UserLoggedOn
- UserLoggedOut

![](/coolstore-microservices/es-context-map-2.png)

## Development: User Story

:::tip
Based on the business context to define User Stories
:::

#### Product Catalog

- As a Buyer, I want to see the list of products with filtering, sorting on the home page (name, photo, short description, rating, and hot product flag which is a product with a lot of people see or buy).
  - Whenever filtering with any price and name of the product, then the list of products need to narrow down with appropriate products.
  - Whenever sorting with descending or ascending on the price or name of the product, then the list of products need to follow with this sorting.
  - Whenever both filtering and sorting in action, then the list of products shall be effective by both of description above.
- As a Buyer, I want to navigate into the detail of one product with the basic attributes such as name, description, available product in the inventory, the inventory store information like stock address and location, a hot product flag (if has) and rating.
- As a SysAdmin, I want to manage a product (CRUD actions) and assign one existing inventory into the product.

#### Shopping Cart

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

#### Payment Process

- Any Buyer can do the payment.
- In the moment of the payment process happening, we start to validation the item information, process for payment and subsequently send an email to the Buyer (because this is just the demo so we don't actually integrate with the payment gateway)
  - Whenever any product information is invalid, then the payment process will be canceled and one email will be sent to Buyer for notification.
  - Whenever ending this payment process, we mark the payment of this cart is processed status and send an email to let Buyer knows.

#### Inventory

- As a SysAdmin, I want to manage inventory (CRUD actions).

#### Rating

- As a Buyer, I want to rate for each product that I think is good (1 -> 5 stars).

#### Access Control

- Each Buyer/SysAdmin is a User.
- As a Buyer/SysAdmin, I want to log-in to the system.
  - Whenever a user with a Buyer role does a login, then I will be brought to the product catalog page.
  - Whenever a user with a SysAdmin role does a login, then I will be brought to the administration page.
- As a Buyer/SysAdmin, I want to log-out of the system.

#### One-off tasks

- Seeding the sample data for the product.
- Seeding the sample data for inventory.
- Seeding the sample data for the rating of a few products (randomness).
- Seeding a Bob user with SysAdmin role.
- Seeding a Mary and Alice users with Buyer role.

## Development: Fine tuning user stories

:::tip
Finding important statements starts with verbs
:::

#### Product Catalog

- As a **Buyer**, I want to **see the list of products with filtering, sorting on the home page** (name, photo, short description, rating, and hot product flag which is a product with a lot of people see or buy).
  - Whenever filtering with any price and name of the product, then the list of products need to narrow down with appropriate products.
  - Whenever sorting with descending or ascending on the price or name of the product, then the list of products need to follow with this sorting.
  - Whenever both filtering and sorting in action, then the list of products shall be effective by both of description above.
- As a **Buyer**, I want to **navigate into the detail of one product** with the basic attributes such as name, description, available product in the inventory, the inventory store information like stock address and location, a hot product flag (if has) and rating.
- As a **SysAdmin**, I want to **manage a product** (CRUD actions) and **assign one existing inventory into the product**.

#### Shopping Cart

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

#### Payment Process

- Any Buyer can do the payment.
- In the moment of the **payment process** happening, we start to **validation the item information**, **process for payment** and subsequently **send an email to the Buyer** (because this is just the demo so we don't actually integrate with the payment gateway)
  - Whenever any product information is invalid, then the payment process will be canceled and one email will be sent to Buyer for notification.
  - Whenever ending this payment process, we mark the payment of this cart is processed status and send an email to let Buyer knows.

#### Inventory

- As a **SysAdmin**, I want to **manage inventory** (CRUD actions).

#### Rating

- As a **Buyer**, I want to **rate for each product** that I think is good (1 -> 5 stars).

#### Access Control

- Each Buyer/SysAdmin is a User.
- As a **Buyer/SysAdmin**, I want to **log-in to the system**.
  - Whenever a user with a Buyer role does a login, then I will be brought to the product catalog page.
  - Whenever a user with a SysAdmin role does a login, then I will be brought to the administration page.
- As a **Buyer/SysAdmin**, I want to **log-out of the system**.

## Development: Rest API

#### Product Catalog

- **Buyer** see the list of products with filtering and sorting on price and name -> get products by filtering and sorting on price and name (**GET**)
- **Buyer** navigate into the detail of one product -> get the detail of specific product (**GET**)
- **SysAdmin** manage product -> create (**POST**), retrieve (**GET**), update (**PUT**) and delete (**DELETE**) a product
- **SysAdmin** assign one existing inventory into the product -> update product with inventory (**PUT**)

#### Shopping Cart

- **Buyer** buy any product on the product catalog page -> create the shopping cart with product (**POST**)
- **Buyer** see the list of products -> get shopping cart with products (**GET**)
- **Buyer** see the summary information panel (at the client side only)
- **Buyer** update the amount of product in the shopping cart (**PUT**)
- **Buyer** delete any product in the shopping cart -> delete product in the shopping cart (**DELETE**)
- **Buyer** check out my shopping cart -> check out the shopping cart (**PUT**)
- **SysAdmin** see shopping cart of all buyers with information -> get shopping cart of buyers (**GET**)
- **SysAdmin** enable/disable any shopping cart of any buyer -> update enabled/disabled shopping cart of buyer (**PUT**)

#### Payment Process

- **System** validation the item information
- **System** process for payment
- **System** send an email to the Buyer

> Background processing with scheduler

#### Inventory

- **SysAdmin** manage inventory -> view (**GET**), create (**POST**), update (**PUT**) and delete (**DELETE**) an inventory

#### Rating

- **Buyer** rate for each product -> create rating for product (**POST**)

#### Access Control

- **User** log in into the system (**POST**)
- **User** log out of the system (**POST**)
