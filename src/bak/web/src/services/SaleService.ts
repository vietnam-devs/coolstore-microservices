import axios, { AxiosResponse, AxiosRequestConfig } from 'axios'

import AuthService from './AuthService'
import { IOrder } from 'stores/types'

const apiUrl = `${process.env.REACT_APP_API}`
const saleResourceUrl = '/sale/api/orders'

const getRequestOptions = (token: string): AxiosRequestConfig => {
  return {
    baseURL: apiUrl,
    data: {},
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`
    }
  }
}

export const getOrders = async () => {
  const user = await AuthService.getUser()
  const response: AxiosResponse = await axios.get<IOrder[]>(
    `${saleResourceUrl}`,
    getRequestOptions(user.access_token)
  )
  console.log(response.data)
  return response.data as IOrder[]
}
