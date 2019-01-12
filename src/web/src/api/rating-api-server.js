import { createAPI } from './create-api-server'

const api = createAPI({
  version: '',
  config: {
    databaseURL: '/rating/api'
  }
})

export function setRating(productId, userId, cost) {
  var model = {
    productId: productId,
    userId: userId,
    cost: cost
  }
  return api.post(`ratings`, model)
}

export function getRating() {
  return api.get(`ratings`)
}
