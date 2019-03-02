import Vue from 'vue'
import Vuex from 'vuex'
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
  state: {},
  actions: {},
  mutations: {},
  getters: {}
})
export default store
