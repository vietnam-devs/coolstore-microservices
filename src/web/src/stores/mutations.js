import Vue from 'vue'
import { setItem, removeItem } from '../helper/storage'

export default {
    SET_LIST: (state, data) => {
        state.products = data
    },
    SET_RATE_ITEM_OF_LIST: (state, itemId, ratting) => {
        // state.products.forEach(product => {
        //     if (product.itemId === itemId) {
        //         product.rating.rating = rating
        //         product.rating.rated = true
        //         product.rating.count++
        //     }
        // })
    },
    SET_CART: (state, data) => {
        state.cart = data
    },
    LOGIN_SUCCESS: (state, data) => {
        state.accessToken = data.accessToken
        state.idToken = data.idToken
        // if (data.accessToken) {
        //     setItem('accessToken', data.accessToken)
        // }
        // if (data.idToken) {
        //     setItem('idToken', data.idToken)
        // }
    },
    LOGOUT: state => {
        delete state.accessToken
        delete state.idToken
    },
    SET_LIST_RATING: (state, data) => {
        state.ratings = data
    }
}
