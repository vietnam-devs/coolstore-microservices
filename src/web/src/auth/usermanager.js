import { oidcSettings } from '../oidcConfig'
import { getItem } from '../helper/storage'
var atob = require('atob')
var applicationUserManager = () => import('oidc-client')
export function login(callback) {
    return applicationUserManager().then(obj => {
        let userManger = new obj.UserManager(oidcSettings)
        callback(userManger.signinRedirect())
    })
}

export function getUser(callback) {
    applicationUserManager().then(obj => {
        let userManger = new obj.UserManager(oidcSettings)
        userManger.getUser().then(response => {
            if (response) {
            } else {
                var userInfo = parseJwt(getItem('idToken'))
                if (userInfo) userInfo = JSON.parse(userInfo)
                callback(userInfo)
            }
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
        callback(userManger.signoutRedirect())
    })
}

function parseJwt(token) {
    if (!token) return null
    var base64Url = token.split('.')[1]
    var base64 = base64Url.replace('-', '+').replace('_', '/')
    return atob(base64)
}
