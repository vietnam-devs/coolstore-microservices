export type Maybe<T> = T | null;

export interface GetProductsInput {
  currentPage: number;

  highPrice: number;
}

export interface GetProductByIdInput {
  productId: string;
}

export interface GetCartInput {
  cartId: string;
}

export interface GetInventoryInput {
  id: string;
}

export interface GetRatingByProductIdInput {
  productId: string;
}

export interface CreateProductInput {
  name: string;

  price: number;

  imageUrl: string;

  desc?: Maybe<string>;
}

export interface InsertItemToNewCartInput {
  productId: string;

  quantity: number;
}

export interface UpdateItemInCartInput {
  cartId: string;

  productId: string;

  quantity: number;
}

export interface DeleteItemInput {
  cartId: string;

  productId: string;
}

export interface CheckoutInput {
  cartId: string;
}

export interface CreateRatingInput {
  productId: string;

  userId: string;

  cost: number;
}

export interface UpdateRatingInput {
  id: string;

  productId: string;

  userId: string;

  cost: number;
}

// ====================================================
// Types
// ====================================================

export interface Query {
  products?: Maybe<(Maybe<Product>)[]>;

  product?: Maybe<Product>;

  carts?: Maybe<Cart>;

  availabilities?: Maybe<(Maybe<Inventory>)[]>;

  availability?: Maybe<Inventory>;

  ratings?: Maybe<(Maybe<Rating>)[]>;

  rating?: Maybe<Rating>;
}

export interface Product {
  id: string;

  name: string;

  price: number;

  imageUrl: string;

  desc?: Maybe<string>;
}

export interface Cart {
  id: string;

  cartItemTotal: number;

  cartItemPromoSavings: number;

  shippingTotal: number;

  shippingPromoSavings: number;

  cartTotal: number;

  isCheckOut: boolean;

  items?: Maybe<(Maybe<CartItem>)[]>;
}

export interface CartItem {
  productId: string;

  productName: string;

  quantity: number;

  price: number;

  promoSavings: number;
}

export interface Inventory {
  id: string;

  location: string;

  quantity: number;

  link: string;
}

export interface Rating {
  id: string;

  productId: string;

  userId: string;

  cost: number;
}

export interface Mutation {
  createProduct: Product;

  insertItemToNewCart: Cart;

  updateItemInCart: Cart;

  deleteItem: string;

  checkout: boolean;

  createRating: Rating;

  updateRating: Rating;
}

// ====================================================
// Arguments
// ====================================================

export interface ProductsQueryArgs {
  input: GetProductsInput;
}
export interface ProductQueryArgs {
  input: GetProductByIdInput;
}
export interface CartsQueryArgs {
  input: GetCartInput;
}
export interface AvailabilityQueryArgs {
  input: GetInventoryInput;
}
export interface RatingQueryArgs {
  input: GetRatingByProductIdInput;
}
export interface CreateProductMutationArgs {
  input: CreateProductInput;
}
export interface InsertItemToNewCartMutationArgs {
  input: InsertItemToNewCartInput;
}
export interface UpdateItemInCartMutationArgs {
  input: UpdateItemInCartInput;
}
export interface DeleteItemMutationArgs {
  input: DeleteItemInput;
}
export interface CheckoutMutationArgs {
  input: CheckoutInput;
}
export interface CreateRatingMutationArgs {
  input: CreateRatingInput;
}
export interface UpdateRatingMutationArgs {
  input: UpdateRatingInput;
}

import { GraphQLResolveInfo } from "graphql";

import { MyContext } from "../context";

export type Resolver<Result, Parent = {}, TContext = {}, Args = {}> = (
  parent: Parent,
  args: Args,
  context: TContext,
  info: GraphQLResolveInfo
) => Promise<Result> | Result;

export interface ISubscriptionResolverObject<Result, Parent, TContext, Args> {
  subscribe<R = Result, P = Parent>(
    parent: P,
    args: Args,
    context: TContext,
    info: GraphQLResolveInfo
  ): AsyncIterator<R | Result> | Promise<AsyncIterator<R | Result>>;
  resolve?<R = Result, P = Parent>(
    parent: P,
    args: Args,
    context: TContext,
    info: GraphQLResolveInfo
  ): R | Result | Promise<R | Result>;
}

export type SubscriptionResolver<
  Result,
  Parent = {},
  TContext = {},
  Args = {}
> =
  | ((
      ...args: any[]
    ) => ISubscriptionResolverObject<Result, Parent, TContext, Args>)
  | ISubscriptionResolverObject<Result, Parent, TContext, Args>;

export type TypeResolveFn<Types, Parent = {}, TContext = {}> = (
  parent: Parent,
  context: TContext,
  info: GraphQLResolveInfo
) => Maybe<Types>;

export type NextResolverFn<T> = () => Promise<T>;

export type DirectiveResolverFn<TResult, TArgs = {}, TContext = {}> = (
  next: NextResolverFn<TResult>,
  source: any,
  args: TArgs,
  context: TContext,
  info: GraphQLResolveInfo
) => TResult | Promise<TResult>;

export interface QueryResolvers<TContext = MyContext, TypeParent = {}> {
  products?: QueryProductsResolver<
    Maybe<(Maybe<Product>)[]>,
    TypeParent,
    TContext
  >;

  product?: QueryProductResolver<Maybe<Product>, TypeParent, TContext>;

  carts?: QueryCartsResolver<Maybe<Cart>, TypeParent, TContext>;

  availabilities?: QueryAvailabilitiesResolver<
    Maybe<(Maybe<Inventory>)[]>,
    TypeParent,
    TContext
  >;

  availability?: QueryAvailabilityResolver<
    Maybe<Inventory>,
    TypeParent,
    TContext
  >;

  ratings?: QueryRatingsResolver<
    Maybe<(Maybe<Rating>)[]>,
    TypeParent,
    TContext
  >;

  rating?: QueryRatingResolver<Maybe<Rating>, TypeParent, TContext>;
}

export type QueryProductsResolver<
  R = Maybe<(Maybe<Product>)[]>,
  Parent = {},
  TContext = MyContext
> = Resolver<R, Parent, TContext, QueryProductsArgs>;
export interface QueryProductsArgs {
  input: GetProductsInput;
}

export type QueryProductResolver<
  R = Maybe<Product>,
  Parent = {},
  TContext = MyContext
> = Resolver<R, Parent, TContext, QueryProductArgs>;
export interface QueryProductArgs {
  input: GetProductByIdInput;
}

export type QueryCartsResolver<
  R = Maybe<Cart>,
  Parent = {},
  TContext = MyContext
> = Resolver<R, Parent, TContext, QueryCartsArgs>;
export interface QueryCartsArgs {
  input: GetCartInput;
}

export type QueryAvailabilitiesResolver<
  R = Maybe<(Maybe<Inventory>)[]>,
  Parent = {},
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type QueryAvailabilityResolver<
  R = Maybe<Inventory>,
  Parent = {},
  TContext = MyContext
> = Resolver<R, Parent, TContext, QueryAvailabilityArgs>;
export interface QueryAvailabilityArgs {
  input: GetInventoryInput;
}

export type QueryRatingsResolver<
  R = Maybe<(Maybe<Rating>)[]>,
  Parent = {},
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type QueryRatingResolver<
  R = Maybe<Rating>,
  Parent = {},
  TContext = MyContext
> = Resolver<R, Parent, TContext, QueryRatingArgs>;
export interface QueryRatingArgs {
  input: GetRatingByProductIdInput;
}

export interface ProductResolvers<TContext = MyContext, TypeParent = Product> {
  id?: ProductIdResolver<string, TypeParent, TContext>;

  name?: ProductNameResolver<string, TypeParent, TContext>;

  price?: ProductPriceResolver<number, TypeParent, TContext>;

  imageUrl?: ProductImageUrlResolver<string, TypeParent, TContext>;

  desc?: ProductDescResolver<Maybe<string>, TypeParent, TContext>;
}

export type ProductIdResolver<
  R = string,
  Parent = Product,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type ProductNameResolver<
  R = string,
  Parent = Product,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type ProductPriceResolver<
  R = number,
  Parent = Product,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type ProductImageUrlResolver<
  R = string,
  Parent = Product,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type ProductDescResolver<
  R = Maybe<string>,
  Parent = Product,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;

export interface CartResolvers<TContext = MyContext, TypeParent = Cart> {
  id?: CartIdResolver<string, TypeParent, TContext>;

  cartItemTotal?: CartCartItemTotalResolver<number, TypeParent, TContext>;

  cartItemPromoSavings?: CartCartItemPromoSavingsResolver<
    number,
    TypeParent,
    TContext
  >;

  shippingTotal?: CartShippingTotalResolver<number, TypeParent, TContext>;

  shippingPromoSavings?: CartShippingPromoSavingsResolver<
    number,
    TypeParent,
    TContext
  >;

  cartTotal?: CartCartTotalResolver<number, TypeParent, TContext>;

  isCheckOut?: CartIsCheckOutResolver<boolean, TypeParent, TContext>;

  items?: CartItemsResolver<Maybe<(Maybe<CartItem>)[]>, TypeParent, TContext>;
}

export type CartIdResolver<
  R = string,
  Parent = Cart,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type CartCartItemTotalResolver<
  R = number,
  Parent = Cart,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type CartCartItemPromoSavingsResolver<
  R = number,
  Parent = Cart,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type CartShippingTotalResolver<
  R = number,
  Parent = Cart,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type CartShippingPromoSavingsResolver<
  R = number,
  Parent = Cart,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type CartCartTotalResolver<
  R = number,
  Parent = Cart,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type CartIsCheckOutResolver<
  R = boolean,
  Parent = Cart,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type CartItemsResolver<
  R = Maybe<(Maybe<CartItem>)[]>,
  Parent = Cart,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;

export interface CartItemResolvers<
  TContext = MyContext,
  TypeParent = CartItem
> {
  productId?: CartItemProductIdResolver<string, TypeParent, TContext>;

  productName?: CartItemProductNameResolver<string, TypeParent, TContext>;

  quantity?: CartItemQuantityResolver<number, TypeParent, TContext>;

  price?: CartItemPriceResolver<number, TypeParent, TContext>;

  promoSavings?: CartItemPromoSavingsResolver<number, TypeParent, TContext>;
}

export type CartItemProductIdResolver<
  R = string,
  Parent = CartItem,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type CartItemProductNameResolver<
  R = string,
  Parent = CartItem,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type CartItemQuantityResolver<
  R = number,
  Parent = CartItem,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type CartItemPriceResolver<
  R = number,
  Parent = CartItem,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type CartItemPromoSavingsResolver<
  R = number,
  Parent = CartItem,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;

export interface InventoryResolvers<
  TContext = MyContext,
  TypeParent = Inventory
> {
  id?: InventoryIdResolver<string, TypeParent, TContext>;

  location?: InventoryLocationResolver<string, TypeParent, TContext>;

  quantity?: InventoryQuantityResolver<number, TypeParent, TContext>;

  link?: InventoryLinkResolver<string, TypeParent, TContext>;
}

export type InventoryIdResolver<
  R = string,
  Parent = Inventory,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type InventoryLocationResolver<
  R = string,
  Parent = Inventory,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type InventoryQuantityResolver<
  R = number,
  Parent = Inventory,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type InventoryLinkResolver<
  R = string,
  Parent = Inventory,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;

export interface RatingResolvers<TContext = MyContext, TypeParent = Rating> {
  id?: RatingIdResolver<string, TypeParent, TContext>;

  productId?: RatingProductIdResolver<string, TypeParent, TContext>;

  userId?: RatingUserIdResolver<string, TypeParent, TContext>;

  cost?: RatingCostResolver<number, TypeParent, TContext>;
}

export type RatingIdResolver<
  R = string,
  Parent = Rating,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type RatingProductIdResolver<
  R = string,
  Parent = Rating,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type RatingUserIdResolver<
  R = string,
  Parent = Rating,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;
export type RatingCostResolver<
  R = number,
  Parent = Rating,
  TContext = MyContext
> = Resolver<R, Parent, TContext>;

export interface MutationResolvers<TContext = MyContext, TypeParent = {}> {
  createProduct?: MutationCreateProductResolver<Product, TypeParent, TContext>;

  insertItemToNewCart?: MutationInsertItemToNewCartResolver<
    Cart,
    TypeParent,
    TContext
  >;

  updateItemInCart?: MutationUpdateItemInCartResolver<
    Cart,
    TypeParent,
    TContext
  >;

  deleteItem?: MutationDeleteItemResolver<string, TypeParent, TContext>;

  checkout?: MutationCheckoutResolver<boolean, TypeParent, TContext>;

  createRating?: MutationCreateRatingResolver<Rating, TypeParent, TContext>;

  updateRating?: MutationUpdateRatingResolver<Rating, TypeParent, TContext>;
}

export type MutationCreateProductResolver<
  R = Product,
  Parent = {},
  TContext = MyContext
> = Resolver<R, Parent, TContext, MutationCreateProductArgs>;
export interface MutationCreateProductArgs {
  input: CreateProductInput;
}

export type MutationInsertItemToNewCartResolver<
  R = Cart,
  Parent = {},
  TContext = MyContext
> = Resolver<R, Parent, TContext, MutationInsertItemToNewCartArgs>;
export interface MutationInsertItemToNewCartArgs {
  input: InsertItemToNewCartInput;
}

export type MutationUpdateItemInCartResolver<
  R = Cart,
  Parent = {},
  TContext = MyContext
> = Resolver<R, Parent, TContext, MutationUpdateItemInCartArgs>;
export interface MutationUpdateItemInCartArgs {
  input: UpdateItemInCartInput;
}

export type MutationDeleteItemResolver<
  R = string,
  Parent = {},
  TContext = MyContext
> = Resolver<R, Parent, TContext, MutationDeleteItemArgs>;
export interface MutationDeleteItemArgs {
  input: DeleteItemInput;
}

export type MutationCheckoutResolver<
  R = boolean,
  Parent = {},
  TContext = MyContext
> = Resolver<R, Parent, TContext, MutationCheckoutArgs>;
export interface MutationCheckoutArgs {
  input: CheckoutInput;
}

export type MutationCreateRatingResolver<
  R = Rating,
  Parent = {},
  TContext = MyContext
> = Resolver<R, Parent, TContext, MutationCreateRatingArgs>;
export interface MutationCreateRatingArgs {
  input: CreateRatingInput;
}

export type MutationUpdateRatingResolver<
  R = Rating,
  Parent = {},
  TContext = MyContext
> = Resolver<R, Parent, TContext, MutationUpdateRatingArgs>;
export interface MutationUpdateRatingArgs {
  input: UpdateRatingInput;
}

export type AuthorizeDirectiveResolver<Result> = DirectiveResolverFn<
  Result,
  {},
  MyContext
>; /** Directs the executor to skip this field or fragment when the `if` argument is true. */
export type SkipDirectiveResolver<Result> = DirectiveResolverFn<
  Result,
  SkipDirectiveArgs,
  MyContext
>;
export interface SkipDirectiveArgs {
  /** Skipped when true. */
  if: boolean;
}

/** Directs the executor to include this field or fragment only when the `if` argument is true. */
export type IncludeDirectiveResolver<Result> = DirectiveResolverFn<
  Result,
  IncludeDirectiveArgs,
  MyContext
>;
export interface IncludeDirectiveArgs {
  /** Included when true. */
  if: boolean;
}

/** Marks an element of a GraphQL schema as no longer supported. */
export type DeprecatedDirectiveResolver<Result> = DirectiveResolverFn<
  Result,
  DeprecatedDirectiveArgs,
  MyContext
>;
export interface DeprecatedDirectiveArgs {
  /** Explains why this element was deprecated, usually also including a suggestion for how to access supported similar data. Formatted using the Markdown syntax (as specified by [CommonMark](https://commonmark.org/). */
  reason?: string;
}

export type IResolvers<TContext = MyContext> = {
  Query?: QueryResolvers<TContext>;
  Product?: ProductResolvers<TContext>;
  Cart?: CartResolvers<TContext>;
  CartItem?: CartItemResolvers<TContext>;
  Inventory?: InventoryResolvers<TContext>;
  Rating?: RatingResolvers<TContext>;
  Mutation?: MutationResolvers<TContext>;
} & { [typeName: string]: never };

export type IDirectiveResolvers<Result> = {
  authorize?: AuthorizeDirectiveResolver<Result>;
  skip?: SkipDirectiveResolver<Result>;
  include?: IncludeDirectiveResolver<Result>;
  deprecated?: DeprecatedDirectiveResolver<Result>;
} & { [directiveName: string]: never };
