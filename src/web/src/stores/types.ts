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

export interface IAppState {
  user: IAppUser | null
  authenticated: boolean
  accessToken: string | null
  products: IProduct[]
  isProductsLoaded: boolean
  productDetail: IProduct
  isProductLoaded: boolean
}

export interface IAppContextProps {
  state: IAppState
  dispatch: ({ type }: IAction) => void
}
