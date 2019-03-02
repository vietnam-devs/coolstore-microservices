import { oidcSettings } from '../oidcConfig'
var atob = require('atob')
var applicationUserManager = () => import('oidc-client')
import store from '../stores'

export function login(callback) {
  return applicationUserManager().then(obj => {
    let userManger = new obj.UserManager(oidcSettings)
    callback(userManger.signinRedirect())
  })
}

export function getUser() {
  return new Promise((resolve, reject) => {
    applicationUserManager().then(obj => {
      let userManger = new obj.UserManager(oidcSettings)
      resolve(userManger.getUser())
    })
  })
}

export function signinSilent(callback) {
  applicationUserManager().then(obj => {
    let userManger = new obj.UserManager(oidcSettings)
    callback(userManger.signinSilent())
  })
}

export function signoutRedirect(callback) {
  applicationUserManager().then(obj => {
    let userManger = new obj.UserManager(oidcSettings)
    var idToken = store.getters['account/idToken']
    userManger.signoutRedirect({ id_token_hint: idToken }).then(response => {})
    callback(userManger.signoutRedirect({ id_token_hint: idToken }))
  })
}

export function parseJwt(token) {
  if (!token) return null
  var base64Url = token.split('.')[1]
  var base64 = base64Url.replace('-', '+').replace('_', '/')
  return atob(base64)
}
