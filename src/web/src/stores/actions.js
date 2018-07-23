import {
    getProducts,
    setRating,
    checkout,
    setCart,
    getCart,
    addToCard,
    updateCard
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
    CHECKOUT_CART: ({ commit, dispatch, state }, { cartId }) => {
        return checkout(cartId).then(data => {})
    },

    RESET_CART: ({ commit }, authId) => {
        var cart = {
            shoppingCartItemList: []
        }
        var tmpId = getItem('cartId')
        var cartId = null
        if (tmpId && authId) {
            // transfer cart
            cartId = authId
            return setCart(tmpId).then(
                function(result) {
                    removeItem('cartId')
                },
                function(err) {
                    console.log(
                        'could not transfer cart ' +
                            tmpId +
                            ' to cart ' +
                            authId +
                            ': ' +
                            err
                    )
                }
            )
            return
        }

        if (tmpId && !authId) {
            cartId = tmpId
        }

        if (!tmpId && authId) {
            cartId = authId
        }

        if (!tmpId && !authId) {
            tmpId = 'id-' + Math.random()
            setItem('cartId', tmpId)
            cartId = tmpId
        }

        cart.shoppingCartItemList = []
        return getCart(cartId).then(data => {
            cart = data.data
            commit('SET_CART', cart)
        })
    },

    SET_CART: ({}, { cartId }) => {
        return setCart(cartId)
    },

    ADD_TO_CARD: ({ commit, dispatch, state }, { product, quantity }) => {
        var cartId = getItem("cartId");
        if (cartId) {
            return updateCard(cartId, product, quantity);
        } else return addToCard(product, quantity)
    },

    REMOVE_FROM_CARD: ({}, { product, quantity }) => {
        return removeFomCart(product, quantity)
    }
}
