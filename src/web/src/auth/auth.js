import store from '../stores'

export function requireAuth(to, from, next) {
  if (!isLoggedIn()) {
    var path = to.path
    store.commit('account/SET_CALLBACKURL', { path })
    next('/unauthorized')
  } else {
    next()
  }
}

export function isLoggedIn() {
  return store.getters['account/isLoggedIn']
}
