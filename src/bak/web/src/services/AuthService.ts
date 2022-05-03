import { UserManager, UserManagerSettings } from 'oidc-client'
import axios from 'axios'
import { RouteChildrenProps } from 'react-router'

import LoggerService from './LoggerService'

const webUrl = window.location.origin
LoggerService.info(`Web URL is at ${webUrl}.`)

const OidcConfig: UserManagerSettings = {
  client_id: 'coolstore.web',
  redirect_uri: `${webUrl}/auth/callback`,
  authority: `${process.env.REACT_APP_AUTHORITY}`,
  response_type: 'id_token token',
  post_logout_redirect_uri: `${webUrl}/`,
  scope: 'openid profile scope2',
  silent_redirect_uri: `${webUrl}/auth/silent-renew`,
  automaticSilentRenew: false,
  loadUserInfo: true
}

class AuthService {
  private userManager: UserManager

  constructor() {
    this.userManager = new UserManager(OidcConfig)
  }

  get UserManager(): UserManager {
    return this.userManager
  }

  async getUser() {
    const { data } = await axios.get<any>(`http://localhost:5000/userinfo`)

    console.log(data)
    if(Object.keys(data).length === 0) {
      return null
    }

    return data
  }

  async authenticateUser(location: RouteChildrenProps) {
    // if (!this.userManager || !this.userManager.getUser) {
    //   return
    // }

    // let oidcUser = await this.userManager.getUser()
    // if (!oidcUser || oidcUser.expired) {
    //   LoggerService.debug('user is being authenticated...')
    //   let url = location.location.pathname + (location.location.search || '')
    //   await this.userManager.signinRedirect({ data: { url } })
    // }

    const { data } = await axios.get<any>(`http://localhost:5000/userinfo`)
    if(Object.keys(data).length === 0) {
      window.location.href = "http://localhost:5000/login?redirectUrl=http://localhost:5000"
    }

    return data
  }

  async signOut() {
    window.location.href = "http://localhost:5000/logout?redirectUrl=http://localhost:5000"
    // if (!this.userManager || !this.userManager.getUser) {
    //   return
    // }

    // let oidcUser = await this.userManager.getUser()
    // if (oidcUser) {
    //   LoggerService.info('user is being logged out...')
    //   await this.userManager.signoutRedirect()
    // }
  }
}

export default new AuthService()
