import Vue from 'vue'
import { setItem, removeItem } from '../helper/storage'

export default {
    SET_LIST: (state, data) => {
        state.products = data
    },
    SET_RATE_ITEM_OF_LIST: (state, itemId, ratting) => {
        state.products.forEach(product => {
            if (product.itemId === itemId) {
                product.rating.rating = rating
                product.rating.rated = true
                product.rating.count++
            }
        })
    },
    SET_CART: (state, data) => {
        state.cart = data
    },
    LOGIN_SUCCESS: (state, data) => {
        debugger
        state.accessToken = data.accessToken
        state.idToken = data.idToken
        setItem('accessToken', data.accessToken)
        setItem('idToken', data.idToken)
    },
    LOGOUT: state => {
        debugger
        state.accessToken = undefined
        state.idToken = undefined
        removeItem('accessToken')
        removeItem('idToken')
    }
}
