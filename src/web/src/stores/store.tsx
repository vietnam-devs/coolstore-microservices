// ref https://stackblitz.com/edit/react-ts-tg3gfu
import React, { createContext, useContext, useReducer } from 'react'
import _ from 'lodash'

import { createAction, createActionPayload, ActionsUnion } from './actions'
import { IAppState, IAppContextProps, IAppUser, IProduct, ICart, IUpdateProductInCart } from './types'

export const LOAD_USER_LOGIN = 'LOAD_USER_LOGIN'
export const UNLOAD_USER_LOGIN = 'UNLOAD_USER_LOGIN'
export const LOAD_PRODUCTS = 'LOAD_PRODUCTS'
export const LOAD_PRODUCT = 'LOAD_PRODUCT'
export const LOAD_CART = 'LOAD_CART'
export const UPDATE_PRODUCT_IN_CART = 'UPDATE_PRODUCT_IN_CART'
export const DELETE_PRODUCT_IN_CART = 'DELETE_PRODUCT_IN_CART'
export const SHOW_NOTIFICATION = 'SHOW_NOTIFICATION'
export const HIDE_NOTIFICATION = 'HIDE_NOTIFICATION'

const initialState: IAppState = {
  user: null,
  authenticated: false,
  accessToken: null,
  products: [],
  isProductsLoaded: false,
  productDetail: null,
  isProductLoaded: false,
  cart: null,
  isCartLoaded: false,
  isShowNotification: false,
  notificationMessage: null
}

export const AppActions = {
  loadUserLogin: createActionPayload<typeof LOAD_USER_LOGIN, IAppUser>(LOAD_USER_LOGIN),
  unloadUserLogin: createAction<typeof UNLOAD_USER_LOGIN>(UNLOAD_USER_LOGIN),
  loadProducts: createActionPayload<typeof LOAD_PRODUCTS, IProduct[]>(LOAD_PRODUCTS),
  loadProduct: createActionPayload<typeof LOAD_PRODUCT, IProduct>(LOAD_PRODUCT),
  loadCart: createActionPayload<typeof LOAD_CART, ICart>(LOAD_CART),
  updateProductInCart: createActionPayload<typeof UPDATE_PRODUCT_IN_CART, IUpdateProductInCart>(UPDATE_PRODUCT_IN_CART),
  deleteProductInCart: createActionPayload<typeof DELETE_PRODUCT_IN_CART, string>(DELETE_PRODUCT_IN_CART),
  showNotification: createActionPayload<typeof SHOW_NOTIFICATION, string>(SHOW_NOTIFICATION),
  hideNotification: createAction<typeof HIDE_NOTIFICATION>(HIDE_NOTIFICATION)
}

const reducers = (state: IAppState, action: ActionsUnion<typeof AppActions>) => {
  switch (action.type) {
    case LOAD_USER_LOGIN:
      return {
        ...state,
        user: action.payload,
        authenticated: true,
        accessToken: action.payload.accessToken
      }

    case UNLOAD_USER_LOGIN:
      return {
        ...state,
        user: null,
        authenticated: false
      }

    case LOAD_PRODUCT:
      return {
        ...state,
        productDetail: action.payload,
        isProductLoaded: true
      }

    case LOAD_PRODUCTS:
      return {
        ...state,
        products: action.payload,
        isProductsLoaded: true
      }

    case LOAD_CART:
      return {
        ...state,
        cart: action.payload,
        isCartLoaded: true
      }

    case UPDATE_PRODUCT_IN_CART:
      const tempCart = state.cart
      if (tempCart) {
        const product = _.find(tempCart.items, ['productId', action.payload.productId])
        if (product) {
          product['quantity'] = product.quantity + action.payload.quantity
        }
      }

      return {
        cart: tempCart,
        ...state
      }

    case DELETE_PRODUCT_IN_CART:
      let cartUpdated = state.cart
      if (cartUpdated) {
        const items = _.filter(cartUpdated.items, x => x.productId !== action.payload)
        cartUpdated.items = items
        return {
          ...state,
          cart: cartUpdated
        }
      }

      return { ...state }

    case SHOW_NOTIFICATION:
      return {
        ...state,
        notificationMessage: action.payload,
        isShowNotification: true
      }

    case HIDE_NOTIFICATION:
      return {
        ...state,
        notificationMessage: null,
        isShowNotification: false
      }

    default:
      const exhaustiveCheck: never = action
      if (typeof exhaustiveCheck != 'undefined') break
  }
}

export const AppContext = createContext({} as IAppContextProps)

export const AppProvider = (props: React.Props<IAppContextProps>) => {
  const [state, dispatch] = useReducer(reducers, initialState)
  const value = { state, dispatch } as IAppContextProps
  return <AppContext.Provider value={value}>{props.children}</AppContext.Provider>
}

export const useStore = () => useContext(AppContext)
