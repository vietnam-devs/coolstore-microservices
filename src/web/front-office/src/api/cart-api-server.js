const logRequests = !!process.env.DEBUG_API
import { createAPI } from './create-api-server'

const api = createAPI({
  version: '',
  config: {
    databaseURL: '/cart/api'
  }
})

export function checkout(cartId) {
  return api.post(`carts/${cartId}/checkout`)
}

export function getCart(cartId) {
  return api.get(`carts/${cartId}`)
}

export function addToCard(productId, quantity) {
  var model = {
    productId: productId,
    quantity: quantity
  }
  return api.post(`carts`, model)
}

export function updateCard(cartId, productId, quantity) {
  var model = {
    productId: productId,
    quantity: quantity,
    cartId: cartId
  }
  return api.put(`carts`, model)
}

export function removeFomCart(cartId, productId) {
  return api.delete(`carts/${cartId}/items/${productId}`)
}
