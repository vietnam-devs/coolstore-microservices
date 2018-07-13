const logRequests = !!process.env.DEBUG_API
import {
    createAPI
} from './create-api-server'

const api = createAPI({
    version: '/v2',
    config: {
        databaseURL: '/api'
    }
})

export function checkout(cartId) {
    return api.post(`checkout/${cartId}`)
}

export function setCart(id) {
    return api.post(`cartId/${id}`)
}

export function getCart() {
    return api.get(`cartId`)
}

export function addToCard(product, quantity) {
    return api.post(`cartId/${product.itemId}/${quantity}`)
}

export function removeFomCart(product, quantity) {
    return api.delete(`cartId/${product.itemId}/${quantity}`)
}