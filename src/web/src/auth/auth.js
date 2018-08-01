import store from '../stores'

export function requireAuth(to, from, next) {
    if (!isLoggedIn()) {
        next('/unauthorized')
    } else {
        next()
    }
}

export function isLoggedIn() {
    return store.getters["account/isLoggedIn"]
}
