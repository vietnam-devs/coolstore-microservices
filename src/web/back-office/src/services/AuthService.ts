import { Log, User, UserManager } from 'oidc-client'

export class AuthService {
  public userManager: UserManager

  constructor() {
    const settings = {
      authority: `${process.env.REACT_APP_AUTHORITY}`,
      client_id: 'backoffice',
      redirect_uri: `${process.env.REACT_APP_ROOT_URL}/signin-callback.html`,
      //silent_redirect_uri: `${process.env.REACT_APP_ROOT_URL}/silent-renew.html`,
      post_logout_redirect_uri: `${process.env.REACT_APP_ROOT_URL}`,
      response_type: 'id_token token',
      scope: 'inventory_api_scope cart_api_scope catalog_api_scope rating_api_scope openid profile'
    }
    this.userManager = new UserManager(settings)

    Log.logger = console
    Log.level = Log.INFO
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
