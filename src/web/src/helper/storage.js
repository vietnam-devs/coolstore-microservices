import store from '../stores'

export function getItem(key) {
    if (typeof localStorage !== 'undefined') {
        return localStorage.getItem(key)
    } else {
        return store.state[key]
    }
}

export function setItem(key, value) {
    if (typeof localStorage !== 'undefined') {
        localStorage.setItem(key, value)
    }
    store.state[key] = value
}

export function removeItem(key) {
    if (typeof localStorage !== 'undefined') {
        localStorage.removeItem(key)
    }
    store.state[key] = undefined
}