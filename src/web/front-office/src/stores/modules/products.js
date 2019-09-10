import { getProducts, getProduct, createProduct } from '../../api'

export default {
  namespaced: true,

  state: {
    products: [],
    byIds: [],
    // loading: true,
    // loaded: false,
    error: null,
    page: 1,
    product: {},
    highprice: null
  },

  getters: {
    products: state => {
      return state.products.map(product => {
        let productDefault = {
          price: 0,
          availability: {
            quantity: 0
          },
          rating: {
            rate: 0,
            count: 0
          },
          quantity: 1 //value default when add to cart
        }
        if (!product.availability) {
          product.availability = { ...productDefault.availability }
        }
        return { ...productDefault, ...product }
      })
    },
    highprice: state => {
      state.highprice =
        state.highprice ||
        Math.max.apply(
          Math,
          state.products.map(function(product) {
            return product.price
          })
        )
      return state.highprice
    }
  },

  mutations: {
    GET_LIST_PRODUCT_SUCSESS(state, products) {
      state.products = products
      state.byIds = products.map(product => product.id)
    },

    GET_PRODUCT_FALURE(state, error) {
      state.error = error
    },

    GET_PRODUCT_BY_ID_SUCCESS(state, product) {
      state.product = product.product
    },

    SET_HIGTHEST_PRICE(state, highprice) {
      state.highprice = highprice
    }
  },

  actions: {
    GET_LIST_PRODUCT: ({ commit, dispatch }, { pageIndex, highprice }) => {
      commit('SET_HIGTHEST_PRICE', highprice)
      return new Promise((resolve, reject) => {
        getProducts(pageIndex, highprice)
          .then(
            products => {
              commit('GET_LIST_PRODUCT_SUCSESS', products)
              resolve()
            },
            error => {
              commit('GET_PRODUCT_FALURE', error)
              reject()
            }
          )
          .catch(error => {
            commit('GET_PRODUCT_FALURE', error)
            reject()
          })
      })
    },

    GET_PRODUCT_BY_ID: ({ commit, dispatch }, { productId }) => {
      return new Promise((resolve, reject) => {
        getProduct(productId)
          .then(
            product => {
              commit('GET_PRODUCT_BY_ID_SUCCESS', product)
              resolve()
            },
            error => {
              commit('GET_PRODUCT_FALURE', error)
              reject()
            }
          )
          .catch(error => {
            commit('GET_PRODUCT_FALURE', error)
            reject()
          })
      })
    },

    CREATE_CATEGORY: ({ commit, dispatch, state }, { model }) => {
      return new Promise((resolve, reject) => {
        createProduct(model)
          .then(
            product => {
              commit('GET_PRODUCT_BY_ID_SUCCESS', product)
              resolve()
            },
            error => {
              commit('GET_PRODUCT_FALURE', error)
              reject()
            }
          )
          .catch(error => {
            commit('GET_PRODUCT_FALURE', error)
            reject()
          })
      })
    }
  }
}
