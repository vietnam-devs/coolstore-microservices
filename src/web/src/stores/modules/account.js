import { parseJwt } from '../../auth/usermanager'

export default {
  namespaced: true,

  state: {
    accessToken: null,
    idToken: null,
    callbackUrl: null,
    userInfo: {}
  },

  getters: {
    isLoggedIn: state => {
      return state.accessToken != null
    },

    accessToken: state => {
      return state.accessToken
    },

    idToken: state => {
      return state.accessToken
    },

    userInfo: state => {
      return JSON.parse(parseJwt(state.idToken))
    }
  },

  mutations: {
    LOGIN_SUCCESS: (state, data) => {
      state.accessToken = data.accessToken
      state.idToken = data.idToken
    },
    LOGOUT: state => {
      delete state.accessToken
      delete state.idToken
    },
    SET_CALLBACKURL: (state, data) => {
      state.callbackUrl = data
    }
  },

  actions: {}
}
