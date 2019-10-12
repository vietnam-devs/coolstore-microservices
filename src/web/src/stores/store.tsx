// ref https://stackblitz.com/edit/react-ts-tg3gfu
import React, { createContext, useContext, useReducer } from 'react'
import { IAction, createAction, createActionPayload, ActionsUnion } from './actions'

export const LOAD_USER_LOGIN = 'LOAD_USER_LOGIN'
export const UNLOAD_USER_LOGIN = 'UNLOAD_USER_LOGIN'
export const LOAD_PRODUCTS = 'LOAD_PRODUCTS'

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
}

interface IAppState {
  user: IAppUser | null
  authenticated: boolean
  accessToken: string | null
  products: IProduct[]
}

interface IAppContextProps {
  state: IAppState
  dispatch: ({ type }: IAction) => void
}

const initialState: IAppState = {
  user: null,
  authenticated: false,
  accessToken: null,
  products: []
}

export const AppActions = {
  loadUserLogin: createActionPayload<typeof LOAD_USER_LOGIN, IAppUser>(LOAD_USER_LOGIN),
  unloadUserLogin: createAction<typeof UNLOAD_USER_LOGIN>(UNLOAD_USER_LOGIN),
  loadProducts: createActionPayload<typeof LOAD_PRODUCTS, IProduct[]>(LOAD_PRODUCTS)
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
    case LOAD_PRODUCTS:
      return {
        ...state,
        products: action.payload
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
