import Vue from 'vue'
import Vuex from 'vuex'
import actions from './actions'
import getters from './getters'
import mutations from './mutations'
import cart from './modules/cart'
import products from './modules/products'
import account from './modules/account'
import ratings from './modules/ratings'


Vue.use(Vuex)

const store = new Vuex.Store({
    modules: {
        cart,
        products,
        account,
        ratings
    },
    state: {
        // products: [],
        // cartId: null,
        // cart: {
        //     items: [],
        //     cartTotal: 0,
        //     shippingTotal: 0,
        //     cartItemPromoSavings: 0,
        //     cartItemTotal: 0,
        //     shippingPromoSavings: 0
        // },
        // ratings: []
    },
    actions,
    mutations: {},
    getters: {}
})
export default store
