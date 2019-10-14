import axios, { AxiosResponse, AxiosRequestConfig } from 'axios'

import AuthService from './AuthService'
import { IProduct } from 'stores/types'

const apiUrl = `${process.env.REACT_APP_API}`
const productResourceUrl = '/api/products'

const getRequestOptions = (token: string): AxiosRequestConfig => {
  return {
    baseURL: apiUrl,
    data: {},
    headers: {
      ['Content-Type']: 'application/grpc',
      Authorization: `Bearer ${token}`
    }
  }
}

export const getProducts = async (page: number, price: number) => {
  const user = await AuthService.getUser()
  const response: AxiosResponse = await axios.get<IProduct[]>(
    `${productResourceUrl}/${page}/${price}`,
    getRequestOptions(user.access_token)
  )
  return response.data.products
}

export const getProduct = async (id: string) => {
  const user = await AuthService.getUser()
  const response: AxiosResponse = await axios.get<IProduct>(
    `${productResourceUrl}/${id}`,
    getRequestOptions(user.access_token)
  )
  return response.data.product
}
