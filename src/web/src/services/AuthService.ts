import { UserManager, UserManagerSettings } from 'oidc-client'
import { RouteChildrenProps } from 'react-router'

import LoggerService from './LoggerService'

const webUrl = window.location.origin
LoggerService.info(`Web URL is at ${webUrl}.`)

const OidcConfig: UserManagerSettings = {
  client_id: 'web',
  redirect_uri: `${webUrl}/auth/callback`,
  authority: `${process.env.REACT_APP_AUTHORITY}`,
  response_type: 'id_token token',
  post_logout_redirect_uri: `${webUrl}/`,
  scope: 'openid profile api1',
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
    return await this.userManager.getUser()
  }

  async authenticateUser(location: RouteChildrenProps) {
    if (!this.userManager || !this.userManager.getUser) {
      return
    }

    let oidcUser = await this.userManager.getUser()
    if (!oidcUser || oidcUser.expired) {
      LoggerService.debug('user is being authenticated...')
      let url = location.location.pathname + (location.location.search || '')
      await this.userManager.signinRedirect({ data: { url } })
    }
  }

  async signOut() {
    if (!this.userManager || !this.userManager.getUser) {
      return
    }

    let oidcUser = await this.userManager.getUser()
    if (oidcUser) {
      LoggerService.info('user is being logged out...')
      await this.userManager.signoutRedirect()
    }
  }
}

export default new AuthService()
