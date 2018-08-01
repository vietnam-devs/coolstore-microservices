import { getRating } from '../../api'

export default {
    namespaced: true,

    state: {
        ratings: []
    },

    getters: {
        ratingSet: state => {
            let ratingSet = state.ratings.reduce((obj, item) => {
                obj[item.productId] = item
                return obj
            }, {})
            return state.products.reduce((obj, item) => {
                ratingSet[item.id] = ratingSet[item.id] || {}
                obj[item.id] = ratingSet[item.id]
                return obj
            }, {})
        }
    },

    mutations: {
        GET_LIST_RATING_SUCSESS: (state, data) => {
            state.ratings = data
        },
        GET_LIST_RATING_FALURE: (state, error) => {
            state.error = error
        }
    },

    actions: {
        GET_LIST_RATING: ({ commit }, {}) => {
            return new Promise((resolve, reject) => {
                getRating()
                    .then(
                        ratings => {
                            commit('GET_LIST_RATING_SUCSESS', ratings.value)
                            resolve()
                        },
                        error => {
                            commit('GET_LIST_RATING_FALURE', error)
                            reject()
                        }
                    )
                    .catch(error => {
                        commit('GET_LIST_RATING_FALURE', error)
                        reject()
                    })
            })
        }
    }
}
