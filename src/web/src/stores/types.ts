import { IAction } from './actions'

export interface IAppUser {
  userName: string
  email: string
  accessToken: string
}

export interface IProduct {
  id: string
  name: string
  price: number
  imageUrl: string
  desc: string
}

export interface ICart {
  id: string
  userId: string
  cartItemTotal: number
  cartItemPromoSavings: number
  shippingTotal: number
  shippingPromoSavings: number
  cartTotal: number
  isCheckout: boolean
  items: ICartItem[]
}

export interface ICartItem {
  quantity: number
  price: number
  productId: string
  productName: string
  productPrice: number
  productDesc: string
  productImagePath: string
  inventoryId: string
  inventoryLocation: string
  inventoryDesc: string
  inventoryWebsite: string
}

export interface ICategoryTagModel {
  key: string
  count: number
}

export interface IInventoryTagModel {
  key: string
  count: number
}

export interface IProductSearchResult {
  products: IProduct[]
  categoryTags: ICategoryTagModel[]
  inventoryTags: IInventoryTagModel[]
  totalItem: number
  page: number
}

export interface IUpdateProductInCart {
  productId: string
  quantity: number
}

export interface IAppState {
  user: IAppUser | null
  authenticated: boolean
  accessToken: string | null

  products: IProduct[]
  isProductsLoaded: boolean
  productDetail: IProduct
  isProductLoaded: boolean

  cart: ICart
  isCartLoaded: boolean

  isShowNotification: boolean
  notificationMessage: string
}

export interface IUserStore {
  user: IAppUser | null
  authenticated: boolean
  accessToken: string | null
}

export interface IProductStore {
  products: IProduct[]
  isProductsLoaded: boolean
  productDetail: IProduct
  isProductLoaded: boolean
}

export interface ICartStore {
  cart: ICart
  isCartLoaded: boolean
}

export interface IAppContextProps {
  state: IAppState
  dispatch: ({ type }: IAction) => void
}
