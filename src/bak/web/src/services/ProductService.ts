import axios, { AxiosResponse, AxiosRequestConfig } from 'axios'

import AuthService from './AuthService'
import { IProduct, IProductSearchResult } from 'stores/types'

const apiUrl = `${process.env.REACT_APP_API}`
const productResourceUrl = `${apiUrl}/api-gw/product-catalog/api/products`
const searchProductResourceUrl = `${apiUrl}/api-gw/product-catalog/api/products/search`

// const getRequestOptions = (token: string): AxiosRequestConfig => {
//   return {
//     baseURL: apiUrl,
//     data: {},
//     headers: {
//       'Content-Type': 'application/json',
//       Authorization: `Bearer ${token}`
//     }
//   }
// }

export const getProducts = async (page: number, price: number) => {
  const user = await AuthService.getUser()
  const response: AxiosResponse = await axios.get<IProduct[]>(
    `${productResourceUrl}/${page}/${price}`,
    // getRequestOptions(user.access_token)
  )
  return response.data.products as IProduct[]
}

export const searchProducts = async (query: string, price: number, page: number, pageSize: number = 10) => {
  const user = await AuthService.getUser()
  const response: AxiosResponse = await axios.get<IProduct[]>(
    `${searchProductResourceUrl}/${price}/${page}/${pageSize}`,
    // getRequestOptions(user.access_token)
  )

  const result = {
    products: response.data.results,
    categoryTags: response.data.categoryTags,
    inventoryTags: response.data.inventoryTags,
    page: response.data.page,
    totalItem: response.data.total
  } as IProductSearchResult

  console.log(result)

  return result
}

export const getProduct = async (id: string) => {
  const user = await AuthService.getUser()
  const response: AxiosResponse = await axios.get<IProduct>(
    `${productResourceUrl}/${id}`,
    // getRequestOptions(user.access_token)
  )

  console.log(response)
  return response.data as IProduct
}
