import { checkout, getCart, addToCard, updateCard, removeFomCart } from '../../api'

export default {
  namespaced: true,

  state: {
    cartId: null,
    cart: {
      items: [],
      cartTotal: 0,
      shippingTotal: 0,
      cartItemPromoSavings: 0,
      cartItemTotal: 0,
      shippingPromoSavings: 0
    },
    error: null
  },

  getters: {
    cartReducer: state => {
      state.cart = state.cart || {}
      state.cart.item = state.cart.item || []
      let flat = {
        byItemIds: state.cart.items.map(item => item.productId),
        itemsFlat: state.cart.items.reduce((obj, item) => {
          obj[item.productId] = item
          return obj
        }, {})
      }
      return Object.assign(state.cart, flat)
    },
    cartId: state => {
      return state.cartId
    },
    itemCount: state => {
      state.cart = state.cart || {}
      state.cart.items = state.cart.items || []
      return state.cart.items.length
    }
  },

  mutations: {
    GET_CART_SUCCESS(state, cart) {
      state.cart = cart.result || {}
      state.cartId = cart.result.id
    },

    GET_CART_FALURE(state, error) {
      state.error = error
    },
    REMOVE_FROM_CARD_SUCCESS(state) {}
  },

  actions: {
    CHECKOUT_CART: ({ commit }, { cartId }) => {
      return new Promise((resolve, reject) => {
        checkout(cartId)
          .then(
            data => {
              resolve()
            },
            error => {
              commit('GET_CART_FALURE', error)
              reject()
            }
          )
          .catch(error => {
            commit('GET_CART_FALURE', error)
            reject()
          })
      })
    },

    GET_CART: ({ commit }, { cartId }) => {
      return new Promise((resolve, reject) => {
        getCart(cartId)
          .then(
            cart => {
              commit('GET_CART_SUCCESS', cart)
              resolve()
            },
            error => {
              commit('GET_CART_FALURE', error)
              reject()
            }
          )
          .catch(error => {
            commit('GET_CART_FALURE', error)
            reject()
          })
      })
    },

    ADD_TO_CARD: ({ commit, dispatch, state }, { productId, quantity }) => {
      return new Promise((resolve, reject) => {
        addToCard(productId, quantity)
          .then(
            cart => {
              commit('GET_CART_SUCCESS', cart)
              resolve()
            },
            error => {
              commit('GET_CART_FALURE', error)
              reject()
            }
          )
          .catch(error => {
            commit('GET_CART_FALURE', error)
            reject()
          })
      })
    },

    UPDATE_CARD: ({ commit, dispatch, state }, { cartId, productId, quantity }) => {
      return new Promise((resolve, reject) => {
        updateCard(cartId, productId, quantity)
          .then(
            cart => {
              commit('GET_CART_SUCCESS', cart)
              resolve()
            },
            error => {
              commit('GET_CART_FALURE', error)
              reject()
            }
          )
          .catch(error => {
            commit('GET_CART_FALURE', error)
            reject()
          })
      })
    },

    REMOVE_FROM_CARD: ({ commit, dispatch }, { cartId, productId }) => {
      return new Promise((resolve, reject) => {
        removeFomCart(cartId, productId)
          .then(
            cart => {
              commit('REMOVE_FROM_CARD_SUCCESS', cart)
              dispatch('cart/GET_CART', cartId)
              resolve()
            },
            error => {
              commit('GET_CART_FALURE', error)
              reject()
            }
          )
          .catch(error => {
            commit('GET_CART_FALURE', error)
            reject()
          })
      })
    },

    UPDATE_PRODUCT_QUANTITY: ({ commit }, { cartId, productId, quantity }) => {
      return new Promise((resolve, reject) => {
        updateCard(cartId, productId, quantity)
          .then(
            cart => {
              commit('GET_CART_SUCCESS', cart)
              resolve()
            },
            error => {
              commit('GET_CART_FALURE', error)
              reject()
            }
          )
          .catch(error => {
            commit('GET_CART_FALURE', error)
            reject()
          })
      })
    }
  }
}
