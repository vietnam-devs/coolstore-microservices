const logRequests = !!process.env.DEBUG_API
import { createAPI } from './create-api-server'

const api = createAPI({
  version: '',
  config: {
    databaseURL: '/catalog/api'
  }
})

export function getProducts(pageIndex = 0, highprice = -1) {
  return api.get(`products/${pageIndex}/${highprice}`).then(res => res.products)
}

export function getProduct(id) {
  return api.get(`products/${id}`)
}

export function createProduct(model) {
  return api.post(`products`, model)
}
