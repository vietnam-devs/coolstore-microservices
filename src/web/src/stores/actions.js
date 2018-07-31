import {
    getProducts,
    setRating,
    checkout,
    getCart,
    addToCard,
    updateCard,
    removeFomCart,
    getRating
} from '../api'

import { getItem, removeItem, setItem } from '../helper/storage'

export default {
    // ensure data for rendering given list type
    FETCH_LIST_DATA: ({ commit, dispatch, state }, { pageIndex }) => {
        getProducts(pageIndex).then(response => {
            response = response || {}
            response.data = response.data || {}
            response.data.value = response.data.value || {}
            let products = response.data.value
            getRating().then(response => {
                response = response || {}
                response.data = response.data || []
                console.log(response.data)
                commit('SET_LIST_RATING', response.data)
                commit('SET_LIST', products)
            })
        })
    },

    GET_RATING: ({ commit }, {}) => {},

    // SET_RATING_ITEM: ({ commit }, { itemId, rating }) => {
    //     setRating(itemId, rating).then
    //     ratingSet = ratingSet || {}
    //     commit('SET_RATE_ITEM_OF_LIST', itemId, ratingSet)
    // },

    /* Action of cart*/
    CHECKOUT_CART: ({}) => {
        let cartId = getItem('cartId')
        if (cartId) {
            return checkout(cartId).then(data => {})
        }
    },

    GET_CART: ({ commit }) => {
        let cartId = getItem('cartId')
        if (cartId) {
            return getCart(cartId).then(data => {
                commit('SET_CART', data.data)
            })
        }
    },

    ADD_TO_CARD: ({ commit, dispatch, state }, { productId, quantity }) => {
        let cartId = getItem('cartId')
        if (cartId) {
            return updateCard(cartId, productId, quantity)
        } else return addToCard(productId, quantity)
    },

    REMOVE_FROM_CARD: ({}, { productId }) => {
        let cartId = getItem('cartId')
        if (cartId) {
            return removeFomCart(cartId, productId)
        }
    },

    UPDATE_PRODUCT_QUANTITY: ({}, { productId, quantity }) => {
        let cartId = getItem('cartId')
        return updateCard(cartId, productId, quantity)
    }
}
