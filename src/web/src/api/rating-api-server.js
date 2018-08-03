import { createAPI } from './create-api-server'

var databaseURL = '/api'
if (process.env.NODE_ENV == 'production') {
    databaseURL = '/rat/api'
}

const api = createAPI({
    version: '/v1',
    config: {
        databaseURL: databaseURL
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
