import { Log, User, UserManager } from 'oidc-client'

export class AuthService {
  public userManager: UserManager

  constructor() {
    const settings = {
      authority: process.env.REACT_APP_AUTHORITY,
      client_id: process.env.REACT_APP_CLIENT_ID,
      redirect_uri: `${process.env.REACT_APP_ROOT_URL}/signin-callback.html`,
      silent_redirect_uri: `${process.env.REACT_APP_ROOT_URL}/silent-renew.html`,
      post_logout_redirect_uri: `${process.env.REACT_APP_ROOT_URL}`,
      response_type: 'id_token token',
      scope: process.env.REACT_APP_SCOPES
    }
    this.userManager = new UserManager(settings)

    //Log.logger = console
    //Log.level = Log.INFO
  }

  public getUser = async (): Promise<User> => {
    return await this.userManager.getUser()
  }

  public login = async (): Promise<void> => {
    return this.userManager.signinRedirect()
  }

  public renewToken = async (): Promise<User> => {
    return this.userManager.signinSilent()
  }

  public logout = async (): Promise<void> => {
    return this.userManager.signoutRedirect()
  }
}
