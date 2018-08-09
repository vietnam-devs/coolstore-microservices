import { createAPI } from './create-api-server'

const api = createAPI({
    version: '/v1',
    config: {
        databaseURL: '/rating/api'
    }
})

export function setRating(productId, userId, cost) {
    var model = {
        ProductId: productId,
        UserId: userId,
        Cost: cost
    }
    return api.post(`ratings`, model)
}

export function getRating() {
    return api.get(`ratings`)
}
