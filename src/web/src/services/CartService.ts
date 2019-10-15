import axios, { AxiosResponse, AxiosRequestConfig } from 'axios'

import AuthService from './AuthService'
import { ICart } from 'stores/types'

const apiUrl = `${process.env.REACT_APP_API}`
const cartResourceUrl = '/api/carts'

const getRequestOptions = (token: string): AxiosRequestConfig => {
  return {
    baseURL: apiUrl,
    data: {},
    headers: {
      'Content-Type': 'application/grpc',
      Authorization: `Bearer ${token}`
    }
  }
}

export const getCartForCurrentUser = async () => {
  const user = await AuthService.getUser()
  const response: AxiosResponse = await axios.get<ICart>(
    `${cartResourceUrl}/${user.profile.sub}/cart`,
    getRequestOptions(user.access_token)
  )
  return response.data.cart
}
