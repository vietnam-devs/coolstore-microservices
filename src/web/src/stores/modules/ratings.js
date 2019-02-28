import { getRating, setRating } from '../../api'

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
      return ratingSet
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
    GET_LIST_RATING: ({ commit }) => {
      return new Promise((resolve, reject) => {
        getRating()
          .then(
            ratings => {
              ratings = ratings || {}
              commit('GET_LIST_RATING_SUCSESS', ratings.ratings)
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
    },
    SET_RATING_FOR_PRODUCT: ({ commit }, { productId, userId, cost }) => {
      return new Promise((resolve, reject) => {
        setRating(productId, userId, cost)
          .then(
            ratings => {
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
