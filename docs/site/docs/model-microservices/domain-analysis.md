# Introduction
CoolStore Website has the basic features: Product Catalog, Shopping Cart, Inventory, Rating, Access Control, and System Control.

## Product Catalog
- Buyer can see the list of products on the home page (name, photo, short description, rating, and hot product flag which is a product with a lot of people see or buy). 
- Buyer can navigate into the detail of one specific product, and the detail of product should have some of the basic attributes such as name, description, available product in the inventory, the inventory store information like stock address and location, a hot product flag (if has) and rating.
- Buyer can do a filter the product catalog by price and name to narrow down the items, and she can filter by combination both of 2 criterions.
- Buyer can do a sort on the product catalog by price and name - descending and ascending criterions need to be supported.
- SysAdmin can manage product (CRUD actions) and assign one existing inventory into the product.

## Shopping Cart
- Buyer can click into the buy button in any product on product catalog page to add this product into the shopping cart (1 product will be added by default).
- Buyer can navigate into the product detail and click the buy button to add this product into the shopping cart (1 product will be added by default).
- Buyer can see the shopping cart with products added.
- Buyer can change the number of product in the shopping cart page to adjust the number of any product.
- Buyer can delete any product in the shopping cart which they don't want to buy anymore.
- The cart will be re-calculated after any action like add, update and delete products happened.
- The cart should have summary information for the current session such as cart total cost, promotion item saving cost, subtotal cost, shipping cost, promotion shipping savings cost, total order amount.
- Buyer can click check out button to trigger a checkout process. 
- SysAdmin can see shopping cart of all buyers with information about cart total cost, promotion item saving cost, subtotal cost, shipping cost, promotion shipping savings cost, total order amount.
- SysAdmin can actually enable/disable this current shopping cart of any buyer.

## Inventory
- SysAdmin can manage inventory (CRUD actions).

## Rating
- Buyer can do a rating for each product that she thinks that it is really good (1->5 stars).

## Access Control
- Each Buyer/SysAdmin is a User. 
- User needs to pass the User Registration process to be a Buyer.
- SysAdmin is created one when the system up and running, and then she can create another SysAdmin. 
- Each user has a permission set which defines the particular permission when she invokes particular action in the system. 
- Buyer/SysAdmin can log in into the system.
- Buyer, after login into the system, will be brought to the product catalog page, and cannot access into the administration page.
- SysAdmin, after login into the system, will be brought to the administration page, and whenever she access into the product catalog page, then she cannot do any actions such as buy product, process shopping cart, and checkout process over there.
- Each Buyer/SysAdmin can log out of the system.
- SysAdmin can seed a bob and alice User with Buyer role in the system.

## System Control
- SysAdmin can seed the sample data for the product.
- SysAdmin can seed the sample data for inventory.
- SysAdmin can seed the sample data for the rating of a few products (randomness).
- SysAdmin can see the board of seeded services (product, inventory and rating).
- SysAdmin can see the board of healthy services.

# Fine Tuning 1: Finding verbs

## Product Catalog
- Buyer can [see the list of products] on the home page (name, photo, short description, rating, and hot product flag which is a product with a lot of people see or buy). 
- Buyer can [navigate into the detail of one specific product], and the detail of product should have some of the basic attributes such as name, description, available product in the inventory, the inventory store information like stock address and location, a hot product flag (if has) and rating.
- Buyer can [do a filter the product catalog by price and name] to narrow down the items, and she can filter by combination both of 2 criterions.
- Buyer can [do a sort on the product catalog by price and name] - descending and ascending criterions need to be supported.
- SysAdmin can [manage product] (CRUD actions) and [assign one existing inventory into the product].

## Shopping Cart
- Buyer can click into the buy button in any product on product catalog page to [add this product into the shopping cart] (1 product will be added by default).
- Buyer can navigate into the product detail and click the buy button to [add this product into the shopping cart] (1 product will be added by default).
- Buyer can [see the shopping cart with products added].
- Buyer can [change the number of product in the shopping cart] page to adjust the number of any product.
- Buyer can [delete any product in the shopping cart] which they don't want to buy anymore.
- The cart will be [re-calculated after any action like add, update and delete products happened].
- The cart should [have summary information for the current session] such as cart total cost, promotion item saving cost, subtotal cost, shipping cost, promotion shipping savings cost, total order amount.
- Buyer can click [check out button to trigger a checkout process]. 
- SysAdmin can [see shopping cart of all buyers] with information about cart total cost, promotion item saving cost, subtotal cost, shipping cost, promotion shipping savings cost, total order amount.
- SysAdmin can actually [enable/disable this current shopping cart of any buyer].

## Inventory
- SysAdmin can [manage inventory] (CRUD actions).

## Rating
- Buyer can [do a rating for each product] that she thinks that it is really good (1->5 stars).

## Access Control
- Each Buyer/SysAdmin is a User. 
- User needs to [pass the User Registration process to be a Buyer].
- SysAdmin is [created one when the system up and running], and then she can [create another SysAdmin]. 
- Each user [has a permission set] which defines the particular permission when she invokes particular action in the system. 
- Buyer/SysAdmin can [log in into the system].
- Buyer, after login into the system, will be [brought to the product catalog page], and cannot access into the administration page.
- SysAdmin, after login into the system, will be [brought to the administration page], and whenever she access into the product catalog page, then she cannot do any actions such as buy product, process shopping cart, and checkout process over there.
- Each Buyer/SysAdmin can [log out of the system].
- SysAdmin can [seed a bob and alice User with Buyer role in the system] (in memory mode).

## System Control
- SysAdmin can [seed the sample data for product].
- SysAdmin can [seed the sample data for inventory].
- SysAdmin can [seed the sample data for rating of a few products] (randomness).
- SysAdmin can [see the board of seeded services] (product, inventory and rating).
- SysAdmin can [see the board of healthy services].

# Fine tuning 2: Get rid of un-important parts

## Product Catalog
- [Buyer] see the list of products -> get products
- [Buyer] navigate into the detail of one specific product -> get the detail of specific product
- [Buyer] do a filter the product catalog by price and name -> get products by filtering (price + name)
- [Buyer] do a sort on the product catalog by price and name -> get products by sorting (price + name)
- [SysAdmin] manage product -> create, update and delete a product
- [SysAdmin] assign one existing inventory into the product -> update product with inventory

## Shopping Cart
- [Buyer] add this product into the shopping cart -> create the shopping cart with product
- [Buyer] see the shopping cart with products added -> get shopping cart with products
- [Buyer] change the number of product in the shopping cart -> update the number of product in the shopping cart
- [Buyer] delete any product in the shopping cart
- [Buyer] re-calculated after any action like add, update and delete products happened (at the client side only)
- [Buyer] have summary information for the current session (at the client side only) 
- [Buyer] check out button to trigger a checkout process -> update shopping cart with check out progress 
- [SysAdmin] see shopping cart of all buyers -> get shopping cart for buyers
- [SysAdmin] enable/disable this current shopping cart of any buyer -> update enabled/disabled shopping cart for buyer

## Inventory
- [SysAdmin] manage inventory -> view, create, update and delete an inventory

## Rating
- [Buyer] do a rating for each product -> create rating for product

## Access Control
- [User] pass the User Registration process to be a Buyer
- [User] created SysAdmin when the system up and running
- [SysAdmin] create another SysAdmin
- [User] has a permission set
- [User] log in into the system
- [Buyer] brought to the product catalog page
- [SysAdmin] brought to the administration page
- [User] log out of the system
- [System] seed a bob and alice User with Buyer role in the system

## System Control
- [CronJob] seed the sample data for product
- [CronJob] seed the sample data for inventory
- [CronJob] seed the sample data for rating of a few products
- [SysAdmin] see the board of seeded services
- [SysAdmin] see the board of healthy services

# Usecase View

TODO

# Conceptual Model

TODO

# Event Storming 1: Event

## Product Catalog
- ProductListDisplayed (filter + sorting)
- ProductDisplayed
- ProductCreated
- ProductUpdated
- ProductDeleted
- InventoryAssigned

## Shopping Cart
- ProductInShoppingCartAdded
- ShoppingCartWithProductsDisplayed
- NumberOfProductInShoppingCartChanged
- ProductInShoppingCartDeleted
- ShoppingCartCheckedOut
- BuyersShoppingCartDisplayed
- ShoppingCartEnabled
- ShoppingCartDisabled

## Inventory
- InventoryListDisplayed
- InventoryCreated
- InventoryUpdated
- InventoryDeleted

## Rating
- ProductRated

## Access Control
- BuyerCreated
- SysAdminCreated

## System Control
- ProductSeeded
- InventorySeeded
- RatingSeeded
- ServiceSeededDisplayed
- ServiceHealthyDisplayed

# Event Storming 2: Command

TODO

# Event Storming 3: Invariants

TODO

# Event Storming 4: Bounded Context (Query/View -> Modular)

TODO

# Event Storming 5: Communication between BCs

TODO
