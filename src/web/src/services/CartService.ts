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
  return response.data.cart as ICart
}

export const createCartForCurrentUser = async (productId: string) => {
  const user = await AuthService.getUser()
  const response: AxiosResponse = await axios.post<ICart>(
    `${cartResourceUrl}`,
    {
      productId: productId,
      userId: user.profile.sub,
      quantity: 1
    },
    getRequestOptions(user.access_token)
  )
  return response.data.result as ICart
}

export const updateCartForCurrentUser = async (cartId: string, productId: string, quantity: number) => {
  const user = await AuthService.getUser()
  const response: AxiosResponse = await axios.put<ICart>(
    `${cartResourceUrl}/${cartId}`,
    {
      productId: productId,
      quantity: quantity
    },
    getRequestOptions(user.access_token)
  )
  return response.data.result as ICart
}

export const deleteCartForCurrentUser = async (cartId: string, productId: string) => {
  const user = await AuthService.getUser()
  const response: AxiosResponse = await axios.delete<ICart>(
    `${cartResourceUrl}/${cartId}/items/${productId}`,
    getRequestOptions(user.access_token)
  )
  return response.data.productId as string
}
