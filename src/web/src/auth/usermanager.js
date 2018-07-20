import { oidcSettings } from '../oidcConfig'
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
        userManger.getUser().then(function(user){
            debugger;
        })
        callback(userManger.getUser())
    })
}

export function signinSilent(callback) {
    applicationUserManager().then(obj => {
        let userManger = new obj.UserManager(oidcSettings)
        callback(userManger.signinSilent())
    })
}