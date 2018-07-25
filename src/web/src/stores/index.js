import Vue from 'vue'
import Vuex from 'vuex'
import actions from './actions'
import getters from './getters'
import mutations from './mutations'

Vue.use(Vuex)

const store = new Vuex.Store({
    state: {
        products: [],
        cartId: null,
        cart: {
            items: [],
            cartTotal: 0,
            shippingTotal: 0,
            cartItemPromoSavings: 0,
            cartItemTotal: 0,
            shippingPromoSavings: 0
        },
        isLoggedIn: undefined,
        accessToken: undefined,
        idToken: undefined
    },
    actions,
    mutations,
    getters
})
export default store
