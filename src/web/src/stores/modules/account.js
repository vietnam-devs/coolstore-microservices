export default {
    namespaced: true,

    state: {
        accessToken: null,
        idToken: null,
        callbackUrl: null
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
