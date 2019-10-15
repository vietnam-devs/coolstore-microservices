// ref https://stackblitz.com/edit/react-ts-tg3gfu
import React, { createContext, useContext, useReducer } from 'react'
import { createAction, createActionPayload, ActionsUnion } from './actions'
import { IAppState, IAppContextProps, IAppUser, IProduct, ICart } from './types'

export const LOAD_USER_LOGIN = 'LOAD_USER_LOGIN'
export const UNLOAD_USER_LOGIN = 'UNLOAD_USER_LOGIN'
export const LOAD_PRODUCTS = 'LOAD_PRODUCTS'
export const LOAD_PRODUCT = 'LOAD_PRODUCT'
export const LOAD_CART = 'LOAD_CART'

const initialState: IAppState = {
  user: null,
  authenticated: false,
  accessToken: null,
  products: [],
  isProductsLoaded: false,
  productDetail: null,
  isProductLoaded: false,
  cart: null,
  isCartLoaded: false
}

export const AppActions = {
  loadUserLogin: createActionPayload<typeof LOAD_USER_LOGIN, IAppUser>(LOAD_USER_LOGIN),
  unloadUserLogin: createAction<typeof UNLOAD_USER_LOGIN>(UNLOAD_USER_LOGIN),
  loadProducts: createActionPayload<typeof LOAD_PRODUCTS, IProduct[]>(LOAD_PRODUCTS),
  loadProduct: createActionPayload<typeof LOAD_PRODUCT, IProduct>(LOAD_PRODUCT),
  loadCart: createActionPayload<typeof LOAD_CART, ICart>(LOAD_CART)
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

    default:
      const exhaustiveCheck: never = action
      if (typeof exhaustiveCheck != 'undefined') break
  }
}

export const AppContext = createContext({} as IAppContextProps)

export const AppProvider = (props: any) => {
  const [state, dispatch] = useReducer(reducers, initialState)
  const value = { state, dispatch } as IAppContextProps
  return <AppContext.Provider value={value}>{props.children}</AppContext.Provider>
}

export const useStore = () => useContext(AppContext)
