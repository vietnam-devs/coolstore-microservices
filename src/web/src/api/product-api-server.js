const logRequests = !!process.env.DEBUG_API
import { createAPI } from './create-api-server'

var databaseURL = '/api'
if (process.env.NODE_ENV == 'production') {
    databaseURL = '/cat/api'
}

const api = createAPI({
    version: '/v1',
    config: {
        databaseURL: databaseURL
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
