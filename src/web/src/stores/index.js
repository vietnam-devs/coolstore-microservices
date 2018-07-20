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
            shoppingCartItemList: []
        },
        isLoggedIn: undefined,
        accessToken: undefined,
        idToken: undefined
    },
    actions,
    mutations,
    getters,
})
export default store;
