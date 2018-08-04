const logRequests = !!process.env.DEBUG_API
import { createAPI } from './create-api-server'

const api = createAPI({
    version: '/v1',
    config: {
        databaseURL: '/cat/api'
    }
})

export function getProducts(pageIndex = 0) {
    return api.get(`products?current-page=${pageIndex}`)
}

export function getProduct(id) {
    return api.get(`products/${id}`)
}

export function createProduct(model) {
    return api.post(`products`, model)
}
