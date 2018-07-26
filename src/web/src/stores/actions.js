import {
    getProducts,
    setRating,
    checkout,
    getCart,
    addToCard,
    updateCard,
    removeFomCart
} from '../api'

import { getItem, removeItem, setItem } from '../helper/storage'

export default {
    // ensure data for rendering given list type
    FETCH_LIST_DATA: ({ commit, dispatch, state }, { pageIndex }) => {
        return getProducts(pageIndex).then(reponse => {
            if (reponse && reponse.data && reponse.data && reponse.data.value) {
                commit('SET_LIST', reponse.data.value)
            }
        })
    },

    SET_RATING_ITEM: ({ commit, dispatch, state }, { itemId, rating }) => {
        return setRating(itemId, rating).then(itemId => {
            commit('SET_RATE_ITEM_OF_LIST', itemId, rating)
        })
    },

    /* Action of cart*/
    CHECKOUT_CART: ({ commit }) => {
        var cartId = getItem('cartId')
        if (cartId) {
            return checkout(cartId).then(data => {})
        }
    },

    GET_CART: ({ commit }) => {
        var cartId = getItem('cartId')
        if (cartId) {
            return getCart(cartId).then(data => {
                commit('SET_CART', data.data)
            })
        }
    },

    ADD_TO_CARD: ({ commit, dispatch, state }, { productId, quantity }) => {
        var cartId = getItem('cartId')
        if (cartId) {
            return updateCard(cartId, productId, quantity)
        } else return addToCard(productId, quantity)
    },

    REMOVE_FROM_CARD: ({}, { productId }) => {
        var cartId = getItem('cartId')
        if (cartId) {
            return removeFomCart(cartId, productId)
        }
    },

    UPDATE_PRODUCT_QUANTITY: ({}, { productId, quantity }) => {
        var cartId = getItem('cartId')
        return updateCard(cartId, productId, quantity)
    }
}
