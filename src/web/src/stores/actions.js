import {
    getProducts,
    setRating,
    checkout,
    setCart,
    getCart,
    addToCard
} from '../api'

export default {
    // ensure data for rendering given list type
    FETCH_LIST_DATA: ({
        commit,
        dispatch,
        state
    }, { pageIndex }) => {
        return getProducts(pageIndex)
            .then(products => {
                if (products && products.value) {
                    commit('SET_LIST', products.value);
                }
            })
    },

    SET_RATING_ITEM: ({
        commit,
        dispatch,
        state
    }, { itemId, rating }) => {
        return setRating(itemId, rating)
            .then(itemId => {
                commit('SET_RATE_ITEM_OF_LIST', itemId, rating)
            })
    },

    /* Action of cart*/
    CHECKOUT_CART: (
        {
            commit,
            dispatch,
            state
        }, { cartId }
    ) => {
        return checkout(cartId)
            .then(data => {

            })
    },

    RESET_CART: (
        {
            commit,
            dispatch,
            state
        }, { cartId, authId }
    ) => {
        var cart = {
            shoppingCartItemList: []
        };
        var tmpId = localStorage.getItem('cartId');

        if (tmpId && authId) {
            // transfer cart
            cartId = authId;
            return setCart(tmpId).then(function (result) {
                localStorage.removeItem('cartId');
            }, function (err) {
                console.log("could not transfer cart " + tmpId + " to cart " + authId + ": " + err);
            });
            return;
        }

        if (tmpId && !authId) {
            cartId = tmpId;
        }

        if (!tmpId && authId) {
            cartId = authId;
        }

        if (!tmpId && !authId) {
            tmpId = 'id-' + Math.random();
            localStorage.setItem('cartId', tmpId);
            cartId = tmpId;
        }

        cart.shoppingCartItemList = [];
        return getCart.then(data => {
            cart = data.data;
            commit('SET_CART', cart);
        })
    },

    SET_CART: ({ }, { cartId }
    ) => {
        return setCart(cartId);
    },

    ADD_TO_CARD: ({ }, { product, quantity }
    ) => {
        return addToCard(product, quantity);
    },

    REMOVE_FROM_CARD: ({ }, { product, quantity }
    ) => {
        return removeFomCart(product, quantity);
    },
}
