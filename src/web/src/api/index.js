const logRequests = !!process.env.DEBUG_API
import {
    createAPI
} from './create-api-server'

const api = createAPI({
    version: '/v2',
    config: {
        databaseURL: 'http://localhost:5000/api'
    }
})

export function getProducts(pageIndex = 0) {
    return api.get(`products?CurrentPage=${pageIndex}`)
}

export function watchList(cb, pageIndex = 0) {
    api.get(`products?CurrentPage=${pageIndex}`).then(function (response) {
        if (response && response.value) {
            cb(response.value);
        }
    })
}
