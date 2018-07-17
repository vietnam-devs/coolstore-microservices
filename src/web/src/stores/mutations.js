import Vue from 'vue'

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
        state.accessToken = data
        if (typeof localStorage !== 'undefined') {
            localStorage.setItem('accessToken', data)
        }
    },
    LOGOUT: state => {
        state.accessToken = null
        if (typeof localStorage !== 'undefined') {
            localStorage.removeItem('accessToken')
        }
    }
}
