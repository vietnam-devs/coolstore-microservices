const logRequests = !!process.env.DEBUG_API
import {
    createAPI
} from './create-api-server'

const api = createAPI({
    version: '/v2',
    config: {
        databaseURL: '/rating'
    }
})

export function setRating(itemId, rating) {
    var model = {
        ProductId: itemId, 
        Ratting: rating
    }
    return api.post(`rating`, model)
}
