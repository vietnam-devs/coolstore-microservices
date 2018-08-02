import { getProducts, getProduct } from '../../api'

export default {
    namespaced: true,

    state: {
        products: [],
        byIds: [],
        // loading: true,
        // loaded: false,
        error: null,
        page: 1,
        product: {}
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
                    product.availability = Object.assign(
                        productDefault.availability
                    )
                }
                return Object.assign(productDefault, product)
            })
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
            state.product = product
        }
    },

    actions: {
        GET_LIST_PRODUCT: ({ commit, dispatch }, { pageIndex }) => {
            return new Promise((resolve, reject) => {
                getProducts(pageIndex)
                    .then(
                        products => {
                            commit('GET_LIST_PRODUCT_SUCSESS', products.value)
                            resolve()
                            // dispatch('ratings/GET_LIST_RATING', null, {
                            //     root: true
                            // }).then(response => {
                            //     resolve()
                            // })
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
        }
    }
}
