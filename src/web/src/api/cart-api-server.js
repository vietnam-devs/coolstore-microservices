const logRequests = !!process.env.DEBUG_API
import { createAPI } from './create-api-server'

const api = createAPI({
    version: '/v1',
    config: {
        databaseURL: '/api'
    }
})

export function checkout(cartId) {
    return api.post(`checkout/${cartId}`)
}

export function setCart(id) {
    return api.post(`carts/${id}`)
}

export function getCart(cartId) {
    return api.get(`carts/${cartId}`)
}

export function addToCard(product, quantity) {
    var model = {
        itemId: product.id,
        quantity: quantity
    }
    return api.post(`carts`, model)
}

export function updateCard(cartId, product, quantity) {
    var model = {
        itemId: product.id,
        quantity: quantity
    }
    return api.put(`carts/${cartId}/item/${product.id}`, model)
}

export function removeFomCart(product, quantity) {
    return api.delete(`carts/${product.id}/${quantity}`)
}
