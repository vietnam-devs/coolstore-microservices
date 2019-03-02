import store from '../stores'

export function getItem(key) {
  // if (typeof localStorage !== 'undefined') {
  //     let value = localStorage.getItem(key)
  //     if (typeof value === 'object') {
  //         value = JSON.parse(value)
  //     }
  //     if (value == 'undefined') {
  //         value = undefined
  //     }
  //     return value
  // } else {
  return store.state[key]
  // }
}

export function setItem(key, value) {
  // if (typeof localStorage !== 'undefined') {
  //     if (typeof value === 'object') {
  //         value = JSON.stringify(value)
  //     }
  //     localStorage.setItem(key, value)
  // }
  store.state[key] = value
}

export function removeItem(key) {
  // if (typeof localStorage !== 'undefined') {
  //     localStorage.removeItem(key)
  // }
  store.state[key] = undefined
}
