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

export function removeFomCart(product, quantity) {
    return api.delete(`carts/${product.id}/${quantity}`)
}
